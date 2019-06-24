using System.Linq;

namespace NavyBlueDtos
{

    public static class ResultSerialExtensions
    {
        public static ResultSingleSerialData ToSingle(this ResultSerialData resultSerialData)
        {
            var resultSingleSerialData = new ResultSingleSerialData()
            {
                Item = resultSerialData.Items.Count() > 0 ? resultSerialData.Items.FirstOrDefault() : null,
                EntityTypeName = resultSerialData.EntityTypeName,
                RelatedItems = resultSerialData.RelatedItems
            };
            return resultSingleSerialData;
        }
    }

}