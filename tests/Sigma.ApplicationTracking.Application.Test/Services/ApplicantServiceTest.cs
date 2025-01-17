using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using Sigma.ApplicationTracking.Application.DTOs;
using Sigma.ApplicationTracking.Application.Services;
using Sigma.ApplicationTracking.Domain.Entities;
using Sigma.ApplicationTracking.Domain.Interface;
using Xunit;
namespace Sigma.ApplicationTracking.Application.Test.Services
{
    public class ApplicantServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Applicant>> _mockApplicantRepository;
        private readonly ApplicantService _applicantService;

        public ApplicantServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockApplicantRepository = new Mock<IRepository<Applicant>>(); ;

            // Setup the ApplicantRepository in the UnitOfWork
            _mockUnitOfWork.Setup(uow => uow.ApplicantRepository)
                           .Returns(_mockApplicantRepository.Object);

            _applicantService = new ApplicantService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateOrEditApplicant_NewApplicant_ShouldCreate()
        {
            // Arrange
            var newApplicantDto = new CreateOrUpdateApplicantDTO
            {
                FirstName = "pashupati",
                LastName = "khanal",
                PhoneNumber = "0942180466",
                Email = "pk@mail.com",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://linkedin.com/in/khanaldemo",
                Comment = "New applicant"
            };

            _mockApplicantRepository.Setup(repo => repo.FindAsync(x => x.Email == It.IsAny<string>()))
                                    .ReturnsAsync((Applicant)null); // No applicant found

            // Act
            var result = await _applicantService.CreateOrEditApplicant(newApplicantDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsNewApplicant);
            _mockApplicantRepository.Verify(repo => repo.InsertAsync(It.IsAny<Applicant>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateOrEditApplicant_ExistingApplicant_ShouldUpdate()
        {
            // Arrange
            var existingApplicant = new Applicant
            {
                FirstName = "pashupati",
                LastName = "khanal",
                PhoneNumber = "9842180466",
                Email = "pk@mail.com",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://linkedin.com/in/pk",
                Comment = "Existing applicant"
            };

            var updateApplicantDto = new CreateOrUpdateApplicantDTO
            {
                FirstName = "pashupati Updated",
                LastName = "khanal updated",
                PhoneNumber = "9810410939",
                Email = "pk@mail.com",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://linkedin.com/in/pk",
                Comment = "Updated comment"
            };

            _mockApplicantRepository
                .Setup(repo => repo.FindAsync(It.Is<Expression<Func<Applicant, bool>>>(expr => expr.Compile()(existingApplicant))))
                .ReturnsAsync(existingApplicant);

            _mockApplicantRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Applicant>()))
                .Verifiable();

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync())
                .ReturnsAsync(1); // Fixed to return Task<int>

            // Act
            var result = await _applicantService.CreateOrEditApplicant(updateApplicantDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsNewApplicant);

            _mockApplicantRepository.Verify(
                repo => repo.UpdateAsync(It.Is<Applicant>(a =>
                    a.FirstName == "pashupati Updated" &&
                    a.LastName == "khanal updated" &&
                    a.PhoneNumber == "9810410939" &&
                    a.Email == "pk@mail.com" &&
                    a.Comment == "Updated comment")),
                Times.Once);

            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateOrEditApplicant_NullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _applicantService.CreateOrEditApplicant(null));
        }

        [Fact]
        public async Task CreateOrEditApplicant_RepositoryThrowsException_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var newApplicantDto = new CreateOrUpdateApplicantDTO
            {
                FirstName = "pashupati",
                LastName = "khanal",
                PhoneNumber = "9842180466",
                Email = "pk@mail.com",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://linkedin.com/in/johndoe",
                Comment = "New applicant"
            };

            _mockApplicantRepository.Setup(repo => repo.InsertAsync(It.IsAny<Applicant>()))
                                    .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _applicantService.CreateOrEditApplicant(newApplicantDto));

            Assert.Contains("An error occurred while creating or updating the applicant.", exception.Message);
        }
    }

}