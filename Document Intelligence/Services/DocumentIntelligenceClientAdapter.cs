using Azure;
using Azure.AI.DocumentIntelligence;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Document_Intelligence.Services
{
    internal class DocumentIntelligenceClientAdapter
    {
        private readonly DocumentIntelligenceClient _client;

        public DocumentIntelligenceClientAdapter(string endpoint, string apiKey)
        {
            _client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        }

        public async Task<Operation<AnalyzeResult>> AnalyzeDocumentAsync(string modelId, Stream stream)
        {
            byte[] byteArray;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                byteArray = memoryStream.ToArray();
            }

            BinaryData binaryData = BinaryData.FromBytes(byteArray);
            AnalyzeDocumentContent content = new AnalyzeDocumentContent
            {
                Base64Source = binaryData
            };

            return await _client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, content);
        }
    }
}