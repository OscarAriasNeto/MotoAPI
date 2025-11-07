using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace MotoAPI.Services
{
    public interface IMotoPricePredictionService
    {
        MotoPricePrediction Predict(PredictMotoPriceRequest request);
    }

    public class MotoPricePredictionService : IMotoPricePredictionService
    {
        private readonly Lazy<PredictionEngine<MotoPriceData, MotoPricePrediction>> _predictionEngine;

        public MotoPricePredictionService()
        {
            _predictionEngine = new Lazy<PredictionEngine<MotoPriceData, MotoPricePrediction>>(CreatePredictionEngine);
        }

        public MotoPricePrediction Predict(PredictMotoPriceRequest request)
        {
            var input = new MotoPriceData
            {
                Ano = request.AnoFabricacao,
                Quilometragem = request.Quilometragem,
                Cilindrada = request.Cilindrada
            };

            return _predictionEngine.Value.Predict(input);
        }

        private PredictionEngine<MotoPriceData, MotoPricePrediction> CreatePredictionEngine()
        {
            var context = new MLContext();
            var trainingData = context.Data.LoadFromEnumerable(GetTrainingData());

            var pipeline = context.Transforms.Concatenate("Features",
                    nameof(MotoPriceData.Ano),
                    nameof(MotoPriceData.Quilometragem),
                    nameof(MotoPriceData.Cilindrada))
                .Append(context.Regression.Trainers.Sdca());

            var model = pipeline.Fit(trainingData);
            return context.Model.CreatePredictionEngine<MotoPriceData, MotoPricePrediction>(model);
        }

        private static IEnumerable<MotoPriceData> GetTrainingData()
        {
            yield return new MotoPriceData { Ano = 2020, Quilometragem = 5_000, Cilindrada = 300, ValorDiaria = 180 };
            yield return new MotoPriceData { Ano = 2018, Quilometragem = 15_000, Cilindrada = 250, ValorDiaria = 150 };
            yield return new MotoPriceData { Ano = 2021, Quilometragem = 3_000, Cilindrada = 500, ValorDiaria = 220 };
            yield return new MotoPriceData { Ano = 2016, Quilometragem = 25_000, Cilindrada = 300, ValorDiaria = 130 };
            yield return new MotoPriceData { Ano = 2019, Quilometragem = 10_000, Cilindrada = 400, ValorDiaria = 190 };
        }
    }

    public class MotoPriceData
    {
        [LoadColumn(0)]
        public float Ano { get; set; }

        [LoadColumn(1)]
        public float Quilometragem { get; set; }

        [LoadColumn(2)]
        public float Cilindrada { get; set; }

        [LoadColumn(3), ColumnName("Label")]
        public float ValorDiaria { get; set; }
    }

    public class MotoPricePrediction
    {
        [ColumnName("Score")]
        public float ValorDiaria { get; set; }
    }

    public class PredictMotoPriceRequest
    {
        public float AnoFabricacao { get; set; }

        public float Quilometragem { get; set; }

        public float Cilindrada { get; set; }
    }
}
