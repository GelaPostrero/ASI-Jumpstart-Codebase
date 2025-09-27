using ASI.Basecode.Resources.Constants;
using ASI.Basecode.Services.Utils.Response;
using System.Net;

namespace ASI.Basecode.WebApp.HelperFunctions
{
    public static class CommonHelper
    {
        public static (ResponseModel Response, int StatusCode) GetAddResultMessage(int status)
        {
            if (status >= AppConstants.CrudStatusCodes.Success)
            {
                return (new ResponseModel(Resources.Messages.Common.Added), (int)HttpStatusCode.OK);
            }
            else if (status == AppConstants.CrudStatusCodes.DuplicateExist)
            {
                return (new ResponseModel(Resources.Messages.Common.DuplicateExists), (int)HttpStatusCode.Conflict);
            }
            else
            {
                return (new ResponseModel(Resources.Messages.Common.AddFailed), (int)HttpStatusCode.BadRequest);
            }
        }

        public static (ResponseModel Response, int StatusCode) GetUpdateResultMessage(int status)
        {
            if (status >= AppConstants.CrudStatusCodes.Success)
            {
                return (new ResponseModel(Resources.Messages.Common.Updated), (int)HttpStatusCode.OK);
            }
            else if (status == AppConstants.CrudStatusCodes.DuplicateExist)
            {
                return (new ResponseModel(Resources.Messages.Common.UpdateFieldUniqueExists), (int)HttpStatusCode.Conflict);
            }
            else if (status == AppConstants.CrudStatusCodes.DoesNotExist)
            {
                return (new ResponseModel(Resources.Messages.Common.UpdateDoesNotExist), (int)HttpStatusCode.NotFound);
            }
            else
            {
                return (new ResponseModel(Resources.Messages.Common.UpdateFailed), (int)HttpStatusCode.BadRequest);
            }
        }

        public static string[] GetResultCount(int[] statusList, string operation)
        {
            // 8 Messages for batch process
            var successCount = 0;
            var failCount = 0;
            var failDoesNotExistCount = 0;
            var failDuplicateExistCount = 0;
            var failIsInUseCount = 0;
            var failMissingFieldCount = 0;
            var failInvalidFieldCount = 0;
            var failOutdatedCount = 0;

            foreach (var status in statusList)
            {
                if (status >= AppConstants.CrudStatusCodes.Success)
                {
                    successCount++;
                }
                else if (status == AppConstants.CrudStatusCodes.DoesNotExist)
                {
                    failDoesNotExistCount++;
                }
                else if (status == AppConstants.CrudStatusCodes.DuplicateExist)
                {
                    failDuplicateExistCount++;
                }
                else
                {
                    failCount++;
                }
            }

            string successMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.Added,
                AppConstants.Update => Resources.Messages.Common.Updated,
                AppConstants.Delete => Resources.Messages.Common.Deleted,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(successMessage))
            {
                successMessage = char.ToLowerInvariant(successMessage[0]) + successMessage.Substring(1);
            }

            string failMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.AddFailed,
                AppConstants.Update => Resources.Messages.Common.UpdateFailed,
                AppConstants.Delete => Resources.Messages.Common.DeleteFailed,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(successMessage))
            {
                successMessage = char.ToLowerInvariant(successMessage[0]) + successMessage.Substring(1);
            }

            string failDoesNotExistMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.BatchItemDoesNotExist,
                AppConstants.Update => Resources.Messages.Common.UpdateDoesNotExist,
                AppConstants.Delete => Resources.Messages.Common.DeleteDoesNotExist,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failDoesNotExistMessage))
            {
                failDoesNotExistMessage = char.ToLowerInvariant(failDoesNotExistMessage[0]) + failDoesNotExistMessage.Substring(1);
            }

            string failDuplicateExistMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.AddDuplicateExist,
                AppConstants.Update => Resources.Messages.Common.UpdateFieldUniqueExists,
                AppConstants.Delete => Resources.Messages.Common.DeleteFailed,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failDuplicateExistMessage))
            {
                failDuplicateExistMessage = char.ToLowerInvariant(failDuplicateExistMessage[0]) + failDuplicateExistMessage.Substring(1);
            }

            string failIsInUseMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.BatchItemInUse,
                AppConstants.Update => Resources.Messages.Common.UpdateFailedInUse,
                AppConstants.Delete => Resources.Messages.Common.DeleteFailedInUse,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failIsInUseMessage))
            {
                failIsInUseMessage = char.ToLowerInvariant(failIsInUseMessage[0]) + failIsInUseMessage.Substring(1);
            }

            string failMissingFieldMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.InvalidInput,
                AppConstants.Update => Resources.Messages.Common.InvalidInput,
                AppConstants.Delete => Resources.Messages.Common.InvalidInput,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failMissingFieldMessage))
            {
                failMissingFieldMessage = char.ToLowerInvariant(failMissingFieldMessage[0]) + failMissingFieldMessage.Substring(1);
            }

            string failInvalidFieldMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.InvalidInput,
                AppConstants.Update => Resources.Messages.Common.InvalidInput,
                AppConstants.Delete => Resources.Messages.Common.InvalidInput,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failInvalidFieldMessage))
            {
                failInvalidFieldMessage = char.ToLowerInvariant(failInvalidFieldMessage[0]) + failInvalidFieldMessage.Substring(1);
            }

            string failOutdatedMessage = operation switch
            {
                AppConstants.Add => Resources.Messages.Common.AddFailed,
                AppConstants.Update => Resources.Messages.Common.UpdateFailed,
                AppConstants.Delete => Resources.Messages.Common.DeleteFailed,
                _ => string.Empty,
            };

            // Convert the first character to lowercase if the string is not empty
            if (!string.IsNullOrEmpty(failOutdatedMessage))
            {
                failOutdatedMessage = char.ToLowerInvariant(failOutdatedMessage[0]) + failOutdatedMessage.Substring(1);
            }

            var outputList = new string[8];
            if (successCount > 0)
            {
                outputList[0] = $"{successCount} {successMessage}";
            }

            if (failCount > 0)
            {
                outputList[1] = $"{failCount} {failMessage}";
            }

            if (failDoesNotExistCount > 0)
            {
                outputList[2] = $"{failDoesNotExistCount} {failDoesNotExistMessage}";
            }

            if (failDuplicateExistCount > 0)
            {
                outputList[3] = $"{failDuplicateExistCount} {failDuplicateExistMessage}";
            }

            if (failIsInUseCount > 0)
            {
                outputList[4] = $"{failIsInUseCount} {failIsInUseMessage}";
            }

            if (failMissingFieldCount > 0)
            {
                outputList[5] = $"{failMissingFieldCount} {failMissingFieldMessage}";
            }

            if (failInvalidFieldCount > 0)
            {
                outputList[6] = $"{failInvalidFieldCount} {failInvalidFieldMessage}";
            }

            if (failOutdatedCount > 0)
            {
                outputList[7] = $"{failOutdatedCount} {failOutdatedMessage}";
            }

            return outputList;
        }
    }
}
