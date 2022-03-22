﻿namespace Business.Constants;

/// <summary>
///     This class was created to get rid of magic strings and write more readable code.
/// </summary>
public static class Messages
{
    public static string DefaultSuccess => "Success";
    public static string DefaultError => "Unknown Error";

    public static string GroupNotFound => "GroupNotFound";
    public static string UserClaimNotFound => "UserClaimNotFound";
    public static string UserClaimExit => "UserClaimExit";
    public static string OperationClaimNotFound => "OperationClaimNotFound";
    public static string GroupClaimNotFound => "GroupClaimNotFound";
    public static string StringLengthMustBeGreaterThanThree => "StringLengthMustBeGreaterThanThree";
    public static string CouldNotBeVerifyCid => "CouldNotBeVerifyCid";
    public static string VerifyCid => "VerifyCid";
    public static string OperationClaimExists => "OperationClaimExists";
    public static string OperationNotClaimExists => "OperationNotClaimExists";
    public static string AuthorizationsDenied => "AuthorizationsDenied";
    public static string UnauthorizedAccess => "Unauthorized access";
    public static string Added => "Added";
    public static string Deleted => "Deleted";
    public static string Updated => "Updated";
    public static string UserNotFound => "UserNotFound";
    public static string PasswordError => "PasswordError";
    public static string SuccessfulLogin => "SuccessfulLogin";
    public static string SendMobileCode => "SendMobileCode";
    public static string NameAlreadyExist => "NameAlreadyExist";
    public static string EmailAlreadyExist => "EmailAlreadyExist";
    public static string WrongEmail => "WrongEmail";
    public static string CitizenNumber => "CID";
    public static string PasswordEmpty => "PasswordEmpty";
    public static string PasswordLength => "PasswordLength";
    public static string PasswordUppercaseLetter => "PasswordUppercaseLetter";
    public static string PasswordLowercaseLetter => "PasswordLowercaseLetter";
    public static string PasswordDigit => "PasswordDigit";
    public static string PassworddidntMatch => "PassworddidntMatch";
    public static string PasswordSpecialCharacter => "PasswordSpecialCharacter";
    public static string SendPassword => "'Reset password link' sent to email successfully.";
    public static string InvalidCode => "InvalidCode";
    public static string SmsServiceNotFound => "SmsServiceNotFound";
    public static string TrueButCellPhone => "TrueButCellPhone";
    public static string TokenProviderException => "TokenProviderException";
    public static string Unknown => "Unknown";
    public static string NewPassword => "NewPassword";
    public static string ResetPasswordSuccess => "Password changed successfully!";

    public static string ProjectNotFound => "Project not found!";
    public static string GroupClaimExit => "Group claim not found!";
    public static string UserProjectNotFound => "User project not found!";
    public static string UserGroupNotFound => "User Group not found!";
    public static string AlreadyExist => "Already Exist";
    public static string NoContent => "No Content";
    public static string NotFound => "Not Found";
}