using FluentAssertions;
using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Constants;
using ITG.Brix.Users.API.Context.Resources;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using ITG.Brix.Users.API.Context.Services.Responses.Models.Errors;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.IntegrationTests.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITG.Brix.Users.IntegrationTests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private HttpClient _client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
#if DEBUG
            ControllerTestsHelper.InitServer();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
#endif
        }

        [TestInitialize]
        public void TestInitialize()
        {
#if DEBUG
            DatabaseHelper.Init();
            _client = ControllerTestsHelper.GetClient();
#endif
        }

        [TestCleanup]
        public void TestCleanup()
        {
#if DEBUG
            _client.Dispose();
#endif
        }

        #region List

        [TestMethod]
        public async Task ListAllShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";

            await ControllerHelper.CreateUser(login, password, firstName, lastName);

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}", apiVersion));
            var responseBody = await response.Content.ReadAsStringAsync();
            var usersModel = JsonConvert.DeserializeObject<UsersModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            usersModel.Value.Should().HaveCount(1);
            usersModel.Count.Should().Be(1);
            usersModel.NextLink.Should().BeNull();
#endif
        }

        [DataTestMethod]
        [DataRow("firstName eq 'FirstNamea'", 1)]
        [DataRow("firstName ne 'FirstNamea'", 25)]
        [DataRow("lastName eq 'LastNamea'", 1)]
        [DataRow("lastName ne 'LastNamea'", 25)]
        [DataRow("startswith(firstName, 'First') eq true", 26)]
        [DataRow("startswith(firstName, 'Hello') eq false", 26)]
        [DataRow("startswith(firstName, 'First') eq true and endswith(firstName, 'z') eq true", 1)]
        [DataRow("endswith(firstName, 'z') eq true", 1)]
        [DataRow("endswith(firstName, 'z') eq false", 25)]
        [DataRow("substringof('Namea', firstName) eq true", 1)]
        [DataRow("tolower(firstName) eq 'firstnamea'", 1)]
        [DataRow("toupper(firstName) eq 'FIRSTNAMEA'", 1)]
        public async Task ListWithFilterShouldSucceed(string filter, int countResult)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alphabet)
            {
                await ControllerHelper.CreateUser(login + c, password + c, firstName + c, lastName + c);
            }


            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$filter={1}", apiVersion, filter));
            var responseBody = await response.Content.ReadAsStringAsync();
            var usersModel = JsonConvert.DeserializeObject<UsersModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            usersModel.Value.Should().HaveCount(countResult);
            usersModel.Count.Should().Be(countResult);
            usersModel.NextLink.Should().BeNull();
#endif
        }

        [DataTestMethod]
        [DataRow(0, 10, 10)]
        [DataRow(1, 10, 10)]
        [DataRow(10, 100, 16)]
        public async Task ListWithSkipAndTopShouldSucceed(int skip, int top, int countResult)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alphabet)
            {
                await ControllerHelper.CreateUser(login + c, password + c, firstName + c, lastName + c);
            }


            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$skip={1}&$top={2}", apiVersion, skip, top));
            var responseBody = await response.Content.ReadAsStringAsync();
            var usersModel = JsonConvert.DeserializeObject<UsersModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            usersModel.Value.Should().HaveCount(countResult);
            usersModel.Count.Should().Be(countResult);
            usersModel.NextLink.Should().BeNull();
#endif
        }

        [DataTestMethod]
        [DataRow("firstName ne 'FirstNamea'", 0, 10, 10)]
        [DataRow("firstName ne 'FirstNamea'", 0, 30, 25)]
        [DataRow("firstName ne 'FirstNamea'", 0, 10, 10)]
        [DataRow("firstName ne 'FirstNamea'", 20, 10, 5)]
        public async Task ListWithFilterSkipTopShouldSucceed(string filter, int skip, int top, int countResult)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char c in alphabet)
            {
                await ControllerHelper.CreateUser(login + c, password + c, firstName + c, lastName + c);
            }


            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$filter={1}&$skip={2}&$top={3}", apiVersion, filter, skip, top));
            var responseBody = await response.Content.ReadAsStringAsync();
            var usersModel = JsonConvert.DeserializeObject<UsersModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            usersModel.Value.Should().HaveCount(countResult);
            usersModel.Count.Should().Be(countResult);
            usersModel.NextLink.Should().BeNull();
#endif
        }

        [DataTestMethod]
        [DataRow("name eq 'FirstName'")]
        [DataRow("login")]
        public async Task ListShouldFailWhenQueryFilterIsNotValid(string filter)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.InvalidQueryFilter,
                            Message = HandlerFailures.InvalidQueryFilter,
                            Target = Consts.Failure.Detail.Target.QueryFilter
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$filter={1}", apiVersion, filter));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("concat(concat(firstName, ', '), lastName) eq 'FirstName, LastName'")]
        [DataRow("length(firstName) eq 9")]
        [DataRow("replace(firstName, ' ', '') eq 'FirstName'")]
        [DataRow("trim(firstName) eq 'FirstName'")]
        public async Task ListShouldFailWhenQueryFilterHasUnsupportedFunctions(string filter)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.InvalidQueryFilter,
                            Message = HandlerFailures.InvalidQueryFilter,
                            Target = Consts.Failure.Detail.Target.QueryFilter
                        }
                    }
                }
            };


            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$filter={1}", apiVersion, filter));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("    ")]
        public async Task ListShouldSucceedWhenQueryTopPresentButUnset(string top)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$top={1}", apiVersion, top));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
#endif
        }

        [DataTestMethod]
        [DataRow("some invalid value - not a sequence of digits")]
        [DataRow("null")]
        [DataRow("''")]
        [DataRow("'   '")]
        public async Task ListShouldFailWhenQueryTopIsNotValid(string top)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                            {
                                new ResponseErrorField
                                {
                                    Code = Consts.Failure.Detail.Code.InvalidQueryTop,
                                    Message = CustomFailures.TopInvalid,
                                    Target = Consts.Failure.Detail.Target.QueryTop
                                }
                            }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$top={1}", apiVersion, top));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("0")]
        [DataRow("-1")]
        [DataRow("99999999999999999999999")]
        public async Task ListShouldFailWhenQueryTopNotInRange(string top)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                            {
                                new ResponseErrorField
                                {
                                    Code = Consts.Failure.Detail.Code.InvalidQueryTop,
                                    Message = string.Format(CustomFailures.TopRange, Application.Constants.Consts.CqsValidation.TopMaxValue),
                                    Target = Consts.Failure.Detail.Target.QueryTop
                                }
                            }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$top={1}", apiVersion, top));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("    ")]
        public async Task ListShouldSucceedWhenQuerySkipPresentButUnset(string skip)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$skip={1}", apiVersion, skip));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
#endif
        }

        [DataTestMethod]
        [DataRow("some invalid value - not a sequence of digits")]
        [DataRow("null")]
        [DataRow("''")]
        [DataRow("'   '")]
        public async Task ListShouldFailWhenQuerySkipIsNotValid(string skip)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                            {
                                new ResponseErrorField
                                {
                                    Code = Consts.Failure.Detail.Code.InvalidQuerySkip,
                                    Message = CustomFailures.SkipInvalid,
                                    Target = Consts.Failure.Detail.Target.QuerySkip
                                }
                            }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$skip={1}", apiVersion, skip));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("-1")]
        [DataRow("99999999999999999999999")]
        public async Task ListShouldFailWhenQuerySkipNotInRange(string skip)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                            {
                                new ResponseErrorField
                                {
                                    Code = Consts.Failure.Detail.Code.InvalidQuerySkip,
                                    Message = string.Format(CustomFailures.SkipRange, Application.Constants.Consts.CqsValidation.SkipMaxValue),
                                    Target = Consts.Failure.Detail.Target.QuerySkip
                                }
                            }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}&$skip={1}", apiVersion, skip));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }


        [TestMethod]
        public async Task ListShouldFailWhenQueryApiVersionIsMissing()
        {
#if DEBUG
            // Arrange
            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredQueryParameter.Code,
                    Message = ServiceError.MissingRequiredQueryParameter.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync("api/users");
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task ListShouldFailWhenQueryApiVersionIsInvalid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "4.0";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users?api-version={0}", apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        #endregion

        #region Get

        [TestMethod]
        public async Task GetShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "LoginGet";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";

            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var eTag = createdUserResult.ETag;


            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var responseBody = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Headers.ETag.Should().NotBeNull();
            response.Headers.ETag.Tag.Should().Be(eTag);
            userModel.Login.Should().Be(login);
            userModel.FirstName.Should().Be(firstName);
            userModel.LastName.Should().Be(lastName);
#endif
        }

        [TestMethod]
        public async Task GetShouldFailWhenRouteIdIsInvalid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var routeId = "someInvalidId";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = string.Format(RequestFailures.EntityNotFoundByIdentifier,"User"),
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task GetShouldFailWhenRouteIdDoesNotExist()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var routeId = Guid.NewGuid();

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = HandlerFailures.NotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task GetShouldFailWhenRouteIdIsEmptyGuid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var routeId = Guid.Empty.ToString();

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = CustomFailures.UserNotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task GetShouldFailWhenQueryApiVersionIsMissing()
        {
#if DEBUG
            // Arrange
            var routeId = Guid.NewGuid().ToString();

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredQueryParameter.Code,
                    Message = ServiceError.MissingRequiredQueryParameter.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}", routeId));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task GetShouldFailWhenQueryApiVersionIsInvalid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "4.0";
            var routeId = Guid.NewGuid().ToString();

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.GetAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }


        #endregion

        #region Create

        [DataTestMethod]
        [DataRow("FirstNameNew", "LastNameNew")]
        public async Task CreateShouldSucceed(string firstName, string lastName)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = firstName,
                LastName = lastName
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
            response.Headers.ETag.Should().NotBeNull();
            responseBody.Should().BeNullOrEmpty();
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenBodyIsNonJsonContentType()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "text/plain"));
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
            responseBody.Should().BeNullOrEmpty();
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenQueryApiVersionIsMissing()
        {
#if DEBUG
            // Arrange
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredQueryParameter.Code,
                    Message = ServiceError.MissingRequiredQueryParameter.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync("api/users", new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenQueryApiVersionIsInvalid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "4.0";

            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginIsNull()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = null,
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginMandatory,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginIsEmpty()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginMandatory,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginIsIsWhitespace()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "  ",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginMandatory,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "un",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginLength,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = new string('u', 99999999),
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginLength,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginFirstCharacterIsNotLetter()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "#Login",
                Password = "PasswordNew",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginFirstLetter,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLoginAlreadyExists()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            await ControllerHelper.CreateUser(login, password, firstName, lastName);

            var body = new CreateFromBody()
            {
                Login = login,
                Password = "SomePassword$",
                FirstName = "SomeFirstName",
                LastName = "SomeLastName"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceAlreadyExists.Code,
                    Message = ServiceError.ResourceAlreadyExists.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "conflict",
                            Message = HandlerFailures.Conflict,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordIsNull()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = null,
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordMandatory,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordIsEmpty()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordMandatory,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordIsWhitespace()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "   ",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordMandatory,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "p",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordLength,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = new string('p', 9999999),
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordLength,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenPasswordHasNoSpecialCharacterPresent()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "PasswordSecret",
                FirstName = "FirstNameNew",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordSpecial,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task CreateShouldFailWhenFirstNameIsEmpty(string symbols)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = symbols,
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameMandatory,
                            Target = "firstName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenFirstNameLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = "F",
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameLength,
                            Target = "firstName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenFirstNameLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = new string('F', 9999999),
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameLength,
                            Target = "firstName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow("  ")]
        public async Task CreateShouldFailWhenFirstNameContainsSymbolsOtherThanLettersOrLettersAndSingleSpaces(string symbols)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = string.Format("First{0}Name", symbols),
                LastName = "LastNameNew"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed,
                            Target = "firstName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task CreateShouldFailWhenLastNameIsEmpty(string symbols)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = "LastNameNew",
                LastName = symbols,
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameMandatory,
                            Target = "lastName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLastNameLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = "FirstNameNew",
                LastName = "L"
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameLength,
                            Target = "lastName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task CreateShouldFailWhenLastNameLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = "FirstNameNew",
                LastName = new string('L', 9999999)
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameLength,
                            Target = "lastName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow("  ")]
        public async Task CreateShouldFailWhenLastNameContainsSymbolsOtherThanLettersOrLettersAndSingleSpaces(string symbols)
        {
#if DEBUG
            // Arrange
            var apiVersion = "1.0";
            var body = new CreateFromBody()
            {
                Login = "LoginNew",
                Password = "Password$New",
                FirstName = "FirstNameNew",
                LastName = string.Format("First{0}Name", symbols)
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed,
                            Target = "lastName"
                        }
                    }
                }
            };

            // Act
            var response = await _client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        #endregion

        #region Update

        [TestMethod]
        public async Task UpdateShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$Secret";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.Headers.ETag.Should().NotBeNull();
            body.Should().BeNullOrEmpty();
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenBodyIsNonJsonContentType()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.UnsupportedMediaType.Code,
                    Message = ServiceError.UnsupportedMediaType.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Unsupported,
                            Message = string.Format(RequestFailures.HeaderUnsupportedValue, "Content-Type"),
                            Target = Consts.Failure.Detail.Target.ContentType
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "text/plain"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenRouteIdIsInvalid()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = "someInvalidRouteId";
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = string.Format(RequestFailures.EntityNotFoundByIdentifier,"User"),
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenRouteIdIsEmptyGuid()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = Guid.Empty;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";


            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = CustomFailures.UserNotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenRouteIdDoesNotExist()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = Guid.NewGuid();
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = HandlerFaultCode.NotFound.Name,
                            Message = HandlerFailures.NotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenQueryApiVersionIsMissing()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredQueryParameter.Code,
                    Message = ServiceError.MissingRequiredQueryParameter.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}", routeId), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenQueryApiVersionIsInvalid()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "4.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginIsNull()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""login"" : null
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginCannotBeEmpty,
                            Target = "login"
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginIsEmpty()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""login"" : """"
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginCannotBeEmpty,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginIsWhitespace()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""login"" : ""    ""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginCannotBeEmpty,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""login"" : ""L""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginLength,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""login"" : """ + new string('L', 99999999) + @"""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginLength,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public async Task UpdateShouldFailWhenLoginFirstCharacterIsNotLetter(string symbol)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""login"" : """ + symbol + @"LoginNew""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LoginFirstLetter,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLoginAlreadyExists()
        {
#if DEBUG
            // Arrange
            var loginFirst = "LoginFirst";
            var loginSecond = "LoginSecond";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            await ControllerHelper.CreateUser(loginFirst, password, firstName, lastName);
            var createdUserResult = await ControllerHelper.CreateUser(loginSecond, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""login"" : ""LoginFirst""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceAlreadyExists.Code,
                    Message = ServiceError.ResourceAlreadyExists.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "conflict",
                            Message = HandlerFailures.Conflict,
                            Target = "login"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordIsNull()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""password"" : null
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordCannotBeEmpty,
                            Target = "password"
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordIsEmpty()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""password"" : """"
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordCannotBeEmpty,
                            Target = "password"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordIsWhitespace()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""password"" : ""    ""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordCannotBeEmpty,
                            Target = "password"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""password"" : ""P$""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordLength,
                            Target = "password"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""password"" : """ + "P$" + new string('P', 99999999) + @"""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordLength,
                            Target = "password"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenPasswordHasNoSpecialCharacterPresent()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""password"" : ""PasswordNew""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.PasswordSpecial,
                            Target = "password"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenFirstNameLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""firstName"" : ""F""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameLength,
                            Target = "firstName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenFirstNameLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""firstName"" : """ + new string('F', 99999999) + @"""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameLength,
                            Target = "firstName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow("  ")]
        public async Task UpdateShouldFailWhenFirstNameContainsSymbolsOtherThanLettersOrLettersAndSingleSpaces(string symbols)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""firstName"" : ""First" + symbols + @"Name""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed,
                            Target = "firstName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLastNameLengthIsLessThanMinimumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{
                                    ""lastName"" : ""L""
                                 }";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameLength,
                            Target = "lastName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenLastNameLengthIsGreaterThanMaximumAllowed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""lastName"" : """ + new string('L', 99999999) + @"""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameLength,
                            Target = "lastName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow("  ")]
        public async Task UpdateShouldFailWhenLastNameContainsSymbolsOtherThanLettersOrLettersAndSingleSpaces(string symbols)
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;
            var jsonInString = @"{""lastName"" : ""Last" + symbols + @"Name""}";

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidInput.Code,
                    Message = ServiceError.InvalidInput.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = "invalid-input",
                            Message = ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed,
                            Target = "lastName"
                        }
                    }
                }
            };


            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenHeaderIfMatchIsMissing()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";


            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredHeader.Code,
                    Message = ServiceError.MissingRequiredHeader.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.HeaderRequired,"If-Match"),
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                }
            };

            // Act
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task UpdateShouldFailWhenHeaderIfMatchIsWrong()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = "\"8001\"";
            var jsonInString = @"{
                                    ""firstName"" : ""UpdatedFirstName"",
                                    ""lastName"" : ""UpdatedLastName""
                                 }";


            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ConditionNotMet.Code,
                    Message = ServiceError.ConditionNotMet.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = HandlerFaultCode.NotMet.Name,
                            Message = HandlerFailures.NotMet,
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.PatchAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion), new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.PreconditionFailed);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        #endregion

        #region Delete

        [TestMethod]
        public async Task DeleteShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = createdUserResult.ETag;

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            body.Should().BeNullOrEmpty();
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenRouteIdIsInvalid()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = "someInvalidRouteId";
            var ifmatch = createdUserResult.ETag;

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = string.Format(RequestFailures.EntityNotFoundByIdentifier,"User"),
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenRouteIdIsEmptyGuid()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = Guid.Empty;
            var ifmatch = createdUserResult.ETag;


            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = CustomFailures.UserNotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenRouteIdDoesNotExist()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = Guid.NewGuid();
            var ifmatch = createdUserResult.ETag;

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ResourceNotFound.Code,
                    Message = ServiceError.ResourceNotFound.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = HandlerFaultCode.NotFound.Name,
                            Message = HandlerFailures.NotFound,
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenQueryApiVersionIsMissing()
        {
#if DEBUG
            // Arrange
            var routeId = Guid.NewGuid().ToString();

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredQueryParameter.Code,
                    Message = ServiceError.MissingRequiredQueryParameter.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.DeleteAsync(string.Format("api/users/{0}", routeId));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenQueryApiVersionIsInvalid()
        {
#if DEBUG
            // Arrange
            var apiVersion = "4.0";
            var routeId = Guid.NewGuid().ToString();

            var expectedError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidQueryParameterValue.Code,
                    Message = ServiceError.InvalidQueryParameterValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                }
            };

            // Act
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenHeaderIfMatchIsMissing()
        {
#if DEBUG
            // Arrange
            var routeId = Guid.NewGuid().ToString();
            var apiVersion = "1.0";


            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.MissingRequiredHeader.Code,
                    Message = ServiceError.MissingRequiredHeader.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.HeaderRequired,"If-Match"),
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                }
            };

            // Act
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenHeaderIfMatchIsInvalid()
        {
#if DEBUG

            // Arrange
            var routeId = Guid.NewGuid().ToString();
            var apiVersion = "1.0";
            var ifmatch = "\" \"";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.InvalidHeaderValue.Code,
                    Message = ServiceError.InvalidHeaderValue.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.HeaderInvalidValue, "If-Match"),
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                }
            };

            // Act
            //_client.DefaultRequestHeaders.Add("If-Match", "\"8001\"");
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        [TestMethod]
        public async Task DeleteShouldFailWhenHeaderIfMatchIsWrong()
        {
#if DEBUG
            // Arrange
            var login = "Login";
            var password = "Password$";
            var firstName = "FirstName";
            var lastName = "LastName";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);
            var routeId = createdUserResult.Id;
            var ifmatch = "\"8001\"";

            var apiVersion = "1.0";

            var expectedResponseError = new ResponseError
            {
                Error = new ResponseErrorBody
                {
                    Code = ServiceError.ConditionNotMet.Code,
                    Message = ServiceError.ConditionNotMet.Message,
                    Details = new List<ResponseErrorField>
                    {
                        new ResponseErrorField
                        {
                            Code = HandlerFaultCode.NotMet.Name,
                            Message = HandlerFailures.NotMet,
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                }
            };

            // Act
            _client.DefaultRequestHeaders.Add("If-Match", ifmatch);
            var response = await _client.DeleteAsync(string.Format("api/users/{0}?api-version={1}", routeId, apiVersion));
            var body = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ResponseError>(body);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.PreconditionFailed);
            body.Should().NotBeNull();
            error.Should().Be(expectedResponseError);
#endif
        }

        #endregion

        #region Login

        [TestMethod]
        public async Task LoginShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "LoginGood";
            var password = "Password$Good";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);


            var body = new LoginFromBody()
            {
                Login = "LoginGood",
                Password = "Password$Good"
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            // Act
            var response = await _client.PostAsync(string.Format("api/users/login?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            var authenticatedUserModel = JsonConvert.DeserializeObject<AuthenticatedUserModel>(responseBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            authenticatedUserModel.Should().NotBeNull();
            authenticatedUserModel.Id.Should().Be(createdUserResult.Id);
#endif
        }

        [DataTestMethod]
        [DataRow("LoginBad", "Password$Good")]
        [DataRow("LoginGood", "Password$Bad")]
        [DataRow("LoginBad", "Password$Bad")]
        public async Task LoginShouldReturnForbidden(string inputLogin, string inputPassword)
        {
#if DEBUG
            // Arrange
            var login = "LoginGood";
            var password = "Password$Good";
            var firstName = "FirstName";
            var lastName = "LastName";

            var apiVersion = "1.0";
            var createdUserResult = await ControllerHelper.CreateUser(login, password, firstName, lastName);


            var body = new LoginFromBody()
            {
                Login = inputLogin,
                Password = inputPassword
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            // Act
            var response = await _client.PostAsync(string.Format("api/users/login?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

#endif
        }

        #endregion

    }
}
