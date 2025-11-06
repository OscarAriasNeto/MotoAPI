using MotoAPI.Services;
using Xunit;

namespace MotoAPI.Tests;

public class DemandPredictionServiceTests
{
    private readonly DemandPredictionService _service = new();

    [Fact]
    public void Predict_WithTypicalInputs_ReturnsReasonableDemand()
    {
        var request = new DemandPredictionRequest(80f, 10f, 150f);

        var result = _service.Predict(request);

        Assert.True(result.DemandaPrevista > 0f);
        Assert.InRange(result.DemandaPrevista, 0f, 30f);
    }
}
