
using Elfie.Serialization;
using EntityFrameworkCore.Testing.Common;
using FluentAssertions;
using Models.Entities;
using Models.Entities.Identity;
using Moq;
using Tests.Wrappers.Interfaces;

namespace Tests.ExtensionMethod
{
    public class ExtensionMethodTest
    {
        private readonly Mock<IPaginatedService<User>> _paginatedServiceMock;
        public ExtensionMethodTest()
        {
            _paginatedServiceMock = new();
        }

        [Theory]
        [InlineData(1, 10)]
        public async Task ToPaginatedListAsync_Should_Return_List(int pageNumber, int pageSize)
        {
            //Arrange
            var studentList = new AsyncEnumerable<User>(new List<User>
            {
                new User
                {
                    Id=1,
                    Name="Sara",
                    Email="sara@sms.com",
                    PhoneNumber="01012345678",
                    UserOrganizations = new List<UserOrganization>
                    {
                        new UserOrganization
                        {
                            Id = 1,
                            UserId = 1,
                            OrganizationId = 1,
                            Organization = new Organization
                            {
                                Id = 1,
                                Name = "Cairo Org."
                            }
                        }
                    }
                }
            });
            var paginatedResult = PaginatedList<User>.Create(studentList.ToList(), pageNumber, pageSize);
            _paginatedServiceMock.Setup(x => x.ReturnPaginatedResult(studentList, pageNumber, pageSize)).Returns(Task.FromResult(paginatedResult));

            //Act
            var result = await _paginatedServiceMock.Object.ReturnPaginatedResult(studentList, pageNumber, pageSize);

            //Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}
