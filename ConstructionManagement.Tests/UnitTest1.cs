
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConstructionManagement.API.Controllers;
using ConstructionManagement.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ConstructionManagement.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task GetTask_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            // Act
            var result = await controller.GetTask(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            // Act
            var result = await controller.GetTask(99999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasks_ReturnsOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            // Act
            var result = await controller.GetTasks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_ExistingId_ReturnsOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var sampleTask = new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Testing Task",
                ProjectId = 1,
                Status = "Pending"
            };

            context.Tasks.Add(sampleTask);
            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            // Act
            var result = await controller.GetTask(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_EmptyTaskName_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "",
                Description = "Validation test",
                ProjectId = 1,
                Status = "Pending"
            };

            // Act
            var result = await controller.CreateTask(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateTaskDto_TaskNameExceeds150Characters_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = new string('A', 151),
                ProjectId = 1,
                Status = "Pending"
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateTaskDto_DescriptionExceeds500Characters_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = new string('A', 501),
                ProjectId = 1,
                Status = "Pending"
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateTaskDto_StatusExceeds30Characters_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Validation test",
                ProjectId = 1,
                Status = new string('A', 31)
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateTaskDto_InvalidProjectId_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Validation test",
                ProjectId = 0,
                Status = "Pending"
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateTaskDto_EmptyTaskName_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = "",
                Description = "Validation test",
                ProjectId = 1,
                Status = "Pending"
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CreateTaskDto_EmptyStatus_FailsValidation()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Validation test",
                ProjectId = 1,
                Status = ""
            };

            var context = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_DueDateBeforeStartDate_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Date validation test",
                ProjectId = 1,
                Status = "Pending",
                StartDate = new DateOnly(2026, 10, 20),
                DueDate = new DateOnly(2026, 10, 15)
            };

            // Act
            var result = await controller.CreateTask(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonExistingProject_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Project validation test",
                ProjectId = 99999,
                Status = "Pending"
            };

            // Act
            var result = await controller.CreateTask(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonExistingEmployee_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);
            var project = new Project
            {
                ProjectId = 1,
                Location = "Pune"
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Employee validation test",
                ProjectId = 1,
                AssignedTo = 99999,
                Status = "Pending"
            };

            // Act
            var result = await controller.CreateTask(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ValidData_ReturnsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var project = new Project
            {
                ProjectId = 1,
                Location = "Pune"
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Testing Task",
                Description = "Positive unit test",
                ProjectId = 1,
                Status = "Pending"
            };

            // Act
            var result = await controller.CreateTask(dto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(context.Tasks);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ValidData_ReturnsOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var project = new Project
            {
                ProjectId = 1,
                Location = "Pune"
            };

            context.Projects.Add(project);

            var existingTask = new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Old Task",
                Description = "Old Description",
                ProjectId = 1,
                Status = "Pending"
            };

            context.Tasks.Add(existingTask);
            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Updated Description",
                ProjectId = 1,
                Status = "Completed"
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated Task", existingTask.TaskName);
            Assert.Equal("Completed", existingTask.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Invalid ID test",
                ProjectId = 1,
                Status = "Pending"
            };

            // Act
            var result = await controller.UpdateTask(0, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Not found test",
                ProjectId = 1,
                Status = "Pending"
            };

            // Act
            var result = await controller.UpdateTask(99999, dto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_EmptyTaskName_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                Location = "Pune"
            });

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Existing Task",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "",
                Description = "Empty name test",
                ProjectId = 1,
                Status = "Pending"
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_EmptyStatus_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                Location = "Pune"
            });

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Existing Task",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Empty status test",
                ProjectId = 1,
                Status = ""
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_NonExistingProject_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                Location = "Pune"
            });

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Existing Task",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Project validation test",
                ProjectId = 99999,
                Status = "Pending"
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_NonExistingEmployee_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                Location = "Pune"
            });

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Existing Task",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Employee validation test",
                ProjectId = 1,
                AssignedTo = 99999,
                Status = "Pending"
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_DueDateBeforeStartDate_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Projects.Add(new Project
            {
                ProjectId = 1,
                Location = "Pune"
            });

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Existing Task",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            var dto = new CreateTaskDto
            {
                TaskName = "Updated Task",
                Description = "Date validation test",
                ProjectId = 1,
                Status = "Pending",
                StartDate = new DateOnly(2026, 10, 20),
                DueDate = new DateOnly(2026, 10, 15)
            };

            // Act
            var result = await controller.UpdateTask(1, dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            context.Tasks.Add(new ConstructionManagement.API.Models.Task
            {
                TaskId = 1,
                TaskName = "Task To Delete",
                ProjectId = 1,
                Status = "Pending"
            });

            await context.SaveChangesAsync();

            var controller = new TasksController(context);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Tasks);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            // Act
            var result = await controller.DeleteTask(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ConstructionManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ConstructionManagementDbContext(options);

            var controller = new TasksController(context);

            // Act
            var result = await controller.DeleteTask(99999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }



    }
}

