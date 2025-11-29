namespace CleanArchitecture.Api.Endpoints.Users.ChangePasswordEndpoint;

public record ChangePasswordRequest(string Username, string Password, string NewPassword);
