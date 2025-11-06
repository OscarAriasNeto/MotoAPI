using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MotoAPI.Services;

public interface IDemandPredictionService
{
    DemandPredictionResult Predict(DemandPredictionRequest request);
}

public sealed class DemandPredictionService : IDemandPredictionService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public DemandPredictionService()
    {
        _mlContext = new MLContext(seed: 27);

        var trainingData = _mlContext.Data.LoadFromEnumerable(new List<DemandData>
        {
            new() { ValorDiaria = 60f, MediaDiasUso = 12f, KilometragemMediaMensal = 180f, Demanda = 16f },
            new() { ValorDiaria = 75f, MediaDiasUso = 9f, KilometragemMediaMensal = 150f, Demanda = 12f },
            new() { ValorDiaria = 90f, MediaDiasUso = 8f, KilometragemMediaMensal = 140f, Demanda = 10f },
            new() { ValorDiaria = 110f, MediaDiasUso = 6f, KilometragemMediaMensal = 120f, Demanda = 7f },
            new() { ValorDiaria = 55f, MediaDiasUso = 15f, KilometragemMediaMensal = 210f, Demanda = 20f },
            new() { ValorDiaria = 80f, MediaDiasUso = 11f, KilometragemMediaMensal = 170f, Demanda = 14f },
            new() { ValorDiaria = 95f, MediaDiasUso = 7f, KilometragemMediaMensal = 160f, Demanda = 9f },
            new() { ValorDiaria = 45f, MediaDiasUso = 18f, KilometragemMediaMensal = 230f, Demanda = 22f },
        });

        var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(DemandData.ValorDiaria),
                nameof(DemandData.MediaDiasUso),
                nameof(DemandData.KilometragemMediaMensal))
            .Append(_mlContext.Regression.Trainers.Sdca());

        _model = pipeline.Fit(trainingData);
    }

    public DemandPredictionResult Predict(DemandPredictionRequest request)
    {
        var engine = _mlContext.Model.CreatePredictionEngine<DemandData, DemandPrediction>(_model);

        var input = new DemandData
        {
            ValorDiaria = request.ValorDiaria,
            MediaDiasUso = request.MediaDiasUso,
            KilometragemMediaMensal = request.KilometragemMediaMensal
        };

        var prediction = engine.Predict(input);
        var demandaNormalizada = Math.Max(0f, prediction.DemandaPrevista);

        return new DemandPredictionResult((float)Math.Round(demandaNormalizada, 2));
    }

    private sealed class DemandData
    {
        [LoadColumn(0)]
        public float ValorDiaria { get; set; }

        [LoadColumn(1)]
        public float MediaDiasUso { get; set; }

        [LoadColumn(2)]
        public float KilometragemMediaMensal { get; set; }

        [LoadColumn(3)]
        public float Demanda { get; set; }
    }

    private sealed class DemandPrediction
    {
        [ColumnName("Score")]
        public float DemandaPrevista { get; set; }
    }
}

public record DemandPredictionRequest(float ValorDiaria, float MediaDiasUso, float KilometragemMediaMensal);

public record DemandPredictionResult(float DemandaPrevista);
