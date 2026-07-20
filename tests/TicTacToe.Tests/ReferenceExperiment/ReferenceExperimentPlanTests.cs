using TicTacToe.ReferenceExperiment;
using Xunit;

namespace TicTacToe.Tests.ReferenceExperiment;

public class ReferenceExperimentPlanTests
{
    [Fact]
    public void default_plan_should_cover_all_strategy_pairs_with_symbol_alternation()
    {
        ReferenceExperimentPlan plan = ReferenceExperimentPlan.create_default("out");

        Assert.Equal(100, plan.RepetitionsPerScenario);
        Assert.True(plan.AlternateSymbols);
        Assert.True(plan.AlternateFirstParticipant);
        Assert.Equal(6, plan.get_scenarios().Count);
        Assert.Equal(600, plan.RepetitionsPerScenario * plan.get_scenarios().Count);
    }
}
