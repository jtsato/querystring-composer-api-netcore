namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

public readonly struct JsonFrom
{
    public string JsonPath { get; }

    private JsonFrom(string jsonPath)
    {
        JsonPath = jsonPath;
    }

    public static JsonFrom Path(string path)
    {
        return new JsonFrom(path);
    }
}