using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace UnitTest.Core.Domains.QueryStrings.UseCases;

// Inputs fed to the characterization snapshot.
//
// The golden inputs are read straight off the [InlineData] of the golden theory so the corpus can
// never drift from it. Probes are extra inputs that exercise paths the golden suite leaves uncovered:
// they have no asserted expectation, they only pin down the current behaviour so a refactoring that
// changes it becomes visible in the snapshot diff.
[ExcludeFromCodeCoverage]
public static class CharacterizationCorpus
{
    public static IReadOnlyList<string> GoldenInputs()
    {
        MethodInfo method = typeof(BuildQueryStringUseCaseTest)
            .GetMethod(nameof(BuildQueryStringUseCaseTest.SuccessToBuildQueryStringManually));

        return method
            .GetCustomAttributes<InlineDataAttribute>()
            .SelectMany(attribute => attribute.GetData(method))
            .Select(row => (string) row[0])
            .ToList();
    }

    public static IReadOnlyList<string> Probes()
    {
        return new List<string>
        {
            // Two multi-word keywords in the same sentence.
            "Casa no alto da glória ou na bela vista",
            "Sala comercial na vila esperança para alugar",
            "Casa no jardim das américas ou no parque do som",

            // A word that also occurs before the multi-word keyword that contains it.
            "jardim com casa no jardim das américas",
            "centro de casa no centro",

            // The same multi-word keyword twice.
            "Sala comercial e sala comercial no centro",

            // Multi-word keywords together with a countable item: duplicated tokens are invisible on a
            // set of entries but would corrupt a count.
            "Casa no alto da glória e na bela vista com 2 quartos",
            "Casa no alto da glória, na bela vista e no parque do som com 1 quarto e 1 banheiro",

            // Accent handling on substitutions.
            "Apartamento no sao cristovao",
            "Apartamento no são cristóvão",
            "Apartamento no sáo cristovão",

            // Degenerate inputs.
            "",
            "   ",
            "...",
            "?",
            "0",
            "R$",

            // Numerals across scales.
            "de mil a dois mil reais",
            "1 bilhão e 500 milhões",
            "duzentos e cinquenta mil reais",
            "casa de 3 quartos e 3 quartos",

            // Countable item repeated with and without a quantifier.
            "casa com garagem para 2 carros e mais uma garagem",
            "🛏️ 🛏️ 🛏️ 🛏️",
        };
    }

    public static IReadOnlyList<string> All()
    {
        return GoldenInputs().Concat(Probes()).ToList();
    }
}
