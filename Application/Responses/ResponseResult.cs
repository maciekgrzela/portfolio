namespace Application.Responses
{
    public enum ResponseResult
    {
        BadRequestStructure,
        UserIsNotAuthorized,
        AccessDenied,
        ResourceDoesntExist,
        InternalError,
        DataObtained,
        Created,
        Updated,
        Deleted   
    }
}