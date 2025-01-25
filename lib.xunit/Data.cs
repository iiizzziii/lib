using System.Reflection;
using lib.api.Models;
using Xunit.Sdk;

namespace lib.xunit;

public abstract class Data : DataAttribute { }

public class InvalidBookDtoParameters : Data
{
    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        yield return [string.Empty, string.Empty];
        yield return [null!, null!];
        yield return [new string('A', 51), new string('A', 201)];
    }
}

public class InvalidItemIds : Data
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var random = new Random();
        var randomNumbers = Enumerable.Range(0, 5).Select(_ => random.Next(25, 50)).ToList();
        
        foreach (var i in randomNumbers) yield return [i];
    }
}
