using Azure;
using Azure.AI.DocumentIntelligence;
using Document_Intelligence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Document_Intelligence
{
    internal class Program
    {
        public static string filePath = "C:\\Users\\mihai.stancu\\Desktop\\MyOwnWork\\Personal Projects\\documentIntelligence\\Document Intelligence\\Document Intelligence\\Files\\123.PNG";
        public static string endpoint = "https://ms-document-intelligence.cognitiveservices.azure.com/";
        public static string apiKey = "8be6f2cdbbdc4cb89ace205af18431f0";

        static async Task Main(string[] args)
        {
            await Test3();
        }

        public static async Task Test3()
        {
            DocumentProcessor documentProcessor = new DocumentProcessor(endpoint, apiKey);
            await documentProcessor.ProcessDocumentFromPath(filePath);
        }

        public static async Task Test1()
        {
            string endpoint = "https://ms-document-intelligence.cognitiveservices.azure.com/";
            string key = "8be6f2cdbbdc4cb89ace205af18431f0";
            AzureKeyCredential credential = new AzureKeyCredential(key);
            DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
            string filePath = "C:\\Users\\mihai.stancu\\Desktop\\MyOwnWork\\Personal Projects\\documentIntelligence\\Document Intelligence\\Document Intelligence\\Files\\sample-layout.pdf";
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] byteArray;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byteArray = memoryStream.ToArray();
                    BinaryData base64Content = BinaryData.FromBytes(byteArray);

                    AnalyzeDocumentContent content = new AnalyzeDocumentContent
                    {
                        Base64Source = base64Content
                    };

                    Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", content);

                    AnalyzeResult result = operation.Value;
                    for (int i = 0; i < result.Documents.Count; i++)
                    {
                        Console.WriteLine($"Document {i}:");

                        AnalyzedDocument document = result.Documents[i];

                        if (document.Fields.TryGetValue("VendorName", out DocumentField vendorNameField)
                            && vendorNameField.Type == DocumentFieldType.String)
                        {
                            string vendorName = vendorNameField.ValueString;
                            Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}");
                        }

                        if (document.Fields.TryGetValue("CustomerName", out DocumentField customerNameField)
                            && customerNameField.Type == DocumentFieldType.String)
                        {
                            string customerName = customerNameField.ValueString;
                            Console.WriteLine($"Customer Name: '{customerName}', with confidence {customerNameField.Confidence}");
                        }

                        if (document.Fields.TryGetValue("Items", out DocumentField itemsField)
                            && itemsField.Type == DocumentFieldType.List)
                        {
                            foreach (DocumentField itemField in itemsField.ValueList)
                            {
                                Console.WriteLine("Item:");

                                if (itemField.Type == DocumentFieldType.Dictionary)
                                {
                                    IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;

                                    if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField)
                                        && itemDescriptionField.Type == DocumentFieldType.String)
                                    {
                                        string itemDescription = itemDescriptionField.ValueString;
                                        Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                    }

                                    if (itemFields.TryGetValue("Amount", out DocumentField itemAmountField)
                                        && itemAmountField.Type == DocumentFieldType.Currency)
                                    {
                                        CurrencyValue itemAmount = itemAmountField.ValueCurrency;
                                        Console.WriteLine($"  Amount: '{itemAmount.CurrencySymbol}{itemAmount.Amount}', with confidence {itemAmountField.Confidence}");
                                    }
                                }
                            }
                        }

                        if (document.Fields.TryGetValue("SubTotal", out DocumentField subTotalField)
                            && subTotalField.Type == DocumentFieldType.Currency)
                        {
                            CurrencyValue subTotal = subTotalField.ValueCurrency;
                            Console.WriteLine($"Sub Total: '{subTotal.CurrencySymbol}{subTotal.Amount}', with confidence {subTotalField.Confidence}");
                        }

                        if (document.Fields.TryGetValue("TotalTax", out DocumentField totalTaxField)
                            && totalTaxField.Type == DocumentFieldType.Currency)
                        {
                            CurrencyValue totalTax = totalTaxField.ValueCurrency;
                            Console.WriteLine($"Total Tax: '{totalTax.CurrencySymbol}{totalTax.Amount}', with confidence {totalTaxField.Confidence}");
                        }

                        if (document.Fields.TryGetValue("InvoiceTotal", out DocumentField invoiceTotalField)
                            && invoiceTotalField.Type == DocumentFieldType.Currency)
                        {
                            CurrencyValue invoiceTotal = invoiceTotalField.ValueCurrency;
                            Console.WriteLine($"Invoice Total: '{invoiceTotal.CurrencySymbol}{invoiceTotal.Amount}', with confidence {invoiceTotalField.Confidence}");
                        }
                    }
                }
            }
        }

        public static async Task Test2()
        {
            //string endpoint = "https://ms-document-intelligence.cognitiveservices.azure.com/";
            //string apiKey = "8be6f2cdbbdc4cb89ace205af18431f0";

            //DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            //string filePath = "C:\\Users\\mihai.stancu\\Desktop\\MyOwnWork\\Personal Projects\\documentIntelligence\\Document Intelligence\\Document Intelligence\\123.PNG";

            //using (FileStream stream = new FileStream(filePath, FileMode.Open))
            //{
            //    AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", stream);

            //    Azure.AI.FormRecognizer.DocumentAnalysis.AnalyzeResult result = operation.Value;

            //    foreach (Azure.AI.FormRecognizer.DocumentAnalysis.DocumentTable table in result.Tables)
            //    {
            //        Console.WriteLine("Table:");
            //        foreach (Azure.AI.FormRecognizer.DocumentAnalysis.DocumentTableCell cell in table.Cells)
            //        {
            //            Console.WriteLine($"  Cell[{cell.RowIndex},{cell.ColumnIndex}]: '{cell.Content}'");
            //        }
            //    }
            //}
        }
    }
}