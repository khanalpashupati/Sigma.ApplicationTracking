using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sigma.ApplicationTracking.API.Controllers;
using Sigma.ApplicationTracking.Application.DTOs;
using Sigma.ApplicationTracking.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.ApplicationTracking.API.Test.Controllers
{

    public class ApplicantControllerTests
    {
        private readonly Mock<IApplicantService> _applicantServiceMock;
        private readonly ApplicantController _controller;

        public ApplicantControllerTests()
        {
            _applicantServiceMock = new Mock<IApplicantService>();
            _controller = new ApplicantController(_applicantServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrUpdateApplicant_NewApplicant_Returns201Created()
        {
            // Arrange
            var dto = new CreateOrUpdateApplicantDTO
            {
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                Comment = "test comment."
            };

            var resultData = new CreateOrUpdateApplicantDTO
            {
                IsNewApplicant = true,
                Email = dto.Email
            };

            _applicantServiceMock
                .Setup(service => service.CreateOrEditApplicant(dto))
                .ReturnsAsync(resultData);

            // Act
            var result = await _controller.CreateOrUpdateApplicant(dto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Contains("Applicant created successfully.", result.Value.ToString());
        }

        [Fact]
        public async Task CreateOrUpdateApplicant_UpdateExistingApplicant_Returns200Ok()
        {
            // Arrange
            var dto = new CreateOrUpdateApplicantDTO
            {
                Email = "existing@example.com",
                FirstName = "Existing",
                LastName = "User",
                Comment = "Updated applicant."
            };

            var resultData = new CreateOrUpdateApplicantDTO
            {
                IsNewApplicant = false,
                Email = dto.Email
            };

            _applicantServiceMock
                .Setup(service => service.CreateOrEditApplicant(dto))
                .ReturnsAsync(resultData);

            // Act
            var result = await _controller.CreateOrUpdateApplicant(dto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Contains("Applicant updated successfully.", result.Value.ToString());
        }

        [Fact]
        public async Task CreateOrUpdateApplicant_InvalidModelState_Returns400BadRequest()
        {
            // Arrange
            var dto = new CreateOrUpdateApplicantDTO
            {
                Email = "",
                FirstName = "Test",
                LastName = "User",
                Comment = "New applicant."
            };

            _controller.ModelState.AddModelError("Email", "Email cannot be empty, it is required.");

            // Act
            var result = await _controller.CreateOrUpdateApplicant(dto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

            var errors = Assert.IsType<SerializableError>(result.Value);
            Assert.Contains("Email", errors.Keys);
            Assert.Contains("Email cannot be empty, it is required.", ((string[])errors["Email"])[0]);
        }


        [Fact]
        public async Task CreateOrUpdateApplicant_ExceptionThrown_Returns500InternalServerError()
        {
            // Arrange
            var dto = new CreateOrUpdateApplicantDTO
            {
                Email = "error@example.com",
                FirstName = "Error",
                LastName = "Case",
                Comment = "Error case."
            };

            _applicantServiceMock
                .Setup(service => service.CreateOrEditApplicant(dto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.CreateOrUpdateApplicant(dto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", result.Value.ToString());
        }
    }

}
