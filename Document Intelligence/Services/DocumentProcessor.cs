using Azure;
using Azure.AI.DocumentIntelligence;
using Document_Intelligence.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Document_Intelligence.Services
{
    internal class DocumentProcessor
    {
        private readonly DocumentIntelligenceClientAdapter _client;

        public DocumentProcessor(string endpoint, string apiKey)
        {
            _client = new DocumentIntelligenceClientAdapter(endpoint, apiKey);
        }

        public async Task ProcessDocumentFromPath(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync("prebuilt-invoice", stream);

                AnalyzeResult result = operation.Value;
                IDocumentProcessor processor = null;
                if (result.Documents.Count < 0)
                {
                    return;
                }

                AnalyzedDocument document = result.Documents[0];
                switch (document.DocType)
                {
                    case "invoice":
                        processor = new InvoiceProcessor(_client);
                        break;
                }

                processor.ProcessDocument(document);
            }
        }
    }
}