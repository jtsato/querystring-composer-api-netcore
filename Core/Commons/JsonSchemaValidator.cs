using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Commons.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Core.Commons;

public static class JsonSchemaValidator
{
    public static IList<FieldError> Validate(string json, string schema)
    {
        JObject jObject = JObject.Parse(json);
        JSchema jSchema = JSchema.Parse(schema);

        IList<FieldError> fieldErrors = new List<FieldError>();

        AddAllErrors(GetMissingFieldsErrors(jObject, jSchema), fieldErrors);
        AddAllErrors(GetExtraFieldsErrors(jObject, jSchema), fieldErrors);
        AddAllErrors(GetWrongTypeFieldsErrors(jObject, jSchema), fieldErrors);

        return fieldErrors;
    }

    private static void AddAllErrors(List<FieldError> sourceList, ICollection<FieldError> targetList)
    {
        foreach (FieldError fieldError in CollectionsMarshal.AsSpan(sourceList))
        {
            targetList.Add(fieldError);
        }
    }

    private static List<FieldError> GetMissingFieldsErrors(JObject jObject, JSchema jSchema)
    {
        return
        (
            from keyValuePair in jSchema.Properties
            where !jObject.ContainsKey(keyValuePair.Key)
            select new FieldError
            {
                PropertyName = keyValuePair.Key,
                ErrorMessage = "CommonJsonPropertyMissing"
            }
        ).ToList();
    }

    private static List<FieldError> GetExtraFieldsErrors(JObject jObject, JSchema jSchema)
    {
        return (
            from property in jObject.Properties()
            where !jSchema.Properties.ContainsKey(property.Name)
            select new FieldError
            {
                PropertyName = property.Name,
                AttemptedValue = property.Value.ToString(),
                ErrorMessage = "CommonJsonPropertyInvalid"
            }
        ).ToList();
    }

    private static List<FieldError> GetWrongTypeFieldsErrors(JToken jObject, JSchema jSchema)
    {
        IList<ValidationError> schemaErrors = new List<ValidationError>();

        jObject.Validate(jSchema, (_, args) => schemaErrors.Add(args.ValidationError));

        IList<FieldError> fieldErrors =
        (
            from validationError in schemaErrors
            where validationError.ErrorType.Equals(ErrorType.Type)
            select WrongTypeFieldErrorOf(validationError)
        ).ToList();

        return fieldErrors.ToList();
    }

    private static FieldError WrongTypeFieldErrorOf(ValidationError validationError)
    {
        string attemptedValue;
        try
        {
            attemptedValue = (string) validationError.Value;
        }
        catch (Exception)
        {
            attemptedValue = "An invalid value";
        }

        return new FieldError
        {
            PropertyName = validationError.Path,
            AttemptedValue = attemptedValue,
            ErrorMessage = "CommonJsonPropertyWrongType"
        };
    }
}