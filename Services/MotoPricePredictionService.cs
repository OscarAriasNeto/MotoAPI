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
                Modelo = request.Modelo,
                AnoFabricacao = request.AnoFabricacao,
                Quilometragem = request.Quilometragem
            };

            return _predictionEngine.Value.Predict(input);
        }

        private PredictionEngine<MotoPriceData, MotoPricePrediction> CreatePredictionEngine()
        {
            var context = new MLContext();
            var trainingData = context.Data.LoadFromEnumerable(GetTrainingData());

            var pipeline = context.Transforms.Categorical.OneHotEncoding("ModeloEncoded", nameof(MotoPriceData.Modelo))
                .Append(context.Transforms.Concatenate("Features",
                    "ModeloEncoded",
                    nameof(MotoPriceData.AnoFabricacao),
                    nameof(MotoPriceData.Quilometragem)))
                .Append(context.Regression.Trainers.Sdca());

            var model = pipeline.Fit(trainingData);
            return context.Model.CreatePredictionEngine<MotoPriceData, MotoPricePrediction>(model);
        }

        private static IEnumerable<MotoPriceData> GetTrainingData()
        {
            yield return new MotoPriceData { Modelo = "Honda CG 160", AnoFabricacao = 2020, Quilometragem = 5_000, ValorDiaria = 120 };
            yield return new MotoPriceData { Modelo = "Yamaha Fazer 250", AnoFabricacao = 2019, Quilometragem = 12_000, ValorDiaria = 150 };
            yield return new MotoPriceData { Modelo = "Honda PCX 150", AnoFabricacao = 2021, Quilometragem = 4_000, ValorDiaria = 160 };
            yield return new MotoPriceData { Modelo = "BMW G 310 GS", AnoFabricacao = 2022, Quilometragem = 3_500, ValorDiaria = 260 };
            yield return new MotoPriceData { Modelo = "Yamaha NMax 160", AnoFabricacao = 2018, Quilometragem = 18_000, ValorDiaria = 130 };
        }
    }

    public class MotoPriceData
    {
        public string Modelo { get; set; } = string.Empty;

        public float AnoFabricacao { get; set; }

        public float Quilometragem { get; set; }

        [ColumnName("Label")]
        public float ValorDiaria { get; set; }
    }

    public class MotoPricePrediction
    {
        [ColumnName("Score")]
        public float ValorDiaria { get; set; }
    }

    public class PredictMotoPriceRequest
    {
        public string Modelo { get; set; } = string.Empty;

        public float AnoFabricacao { get; set; }

        public float Quilometragem { get; set; }
    }
}
