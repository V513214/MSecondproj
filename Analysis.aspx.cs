using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using text2data.Api.DTO;
using text2data.Api;

namespace Analysis
{
    public partial class Analysis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected string GetResult(string inputText)
        {
            string privateKey = "6FCE9585-5431-40CC-9173-493A5C1A2CD0"; //add your private key here (you can find it in the admin panel once you sign-up)
            string secret = "testcase6";
            string result = "";
            string TempRes = "";

            #region Create request object
            Document doc = new Document()
            {
                DocumentText = inputText,
                IsTwitterContent = false,
                UserCategoryModelName = "", //name of your trained model, leave empty for default
                PrivateKey = privateKey,
                Secret = secret
            };
            #endregion

            DocumentResult docResult = API.GetDocumentAnalysisResult(doc); //execute request

            if (docResult.Status == (int)DocumentResultStatus.OK) //check status
            {
                #region Display results
                //display document level score
                //Console.WriteLine(string.Format("This document is: {0}{1} {2}", docResult.DocSentimentPolarity, docResult.DocSentimentResultString, docResult.DocSentimentValue.ToString("0.000")));
                //TempRes = string.Format("This document is: {0}{1} {2}", docResult.DocSentimentPolarity, docResult.DocSentimentResultString, docResult.DocSentimentValue.ToString("0.000"));
                TempRes = string.Format(docResult.DocSentimentResultString);

                #region display entity scores if any found
                /*if (docResult.Entities != null && docResult.Entities.Any())
                {
                    //Console.WriteLine(Environment.NewLine + "Entities:");
                    TempRes += Environment.NewLine + "Entities:";
                    foreach (var entity in docResult.Entities)
                    {
                        //Console.WriteLine(string.Format("{0} ({1}) {2}{3} {4}", entity.Text, entity.KeywordType, entity.SentimentPolarity, entity.SentimentResult, entity.SentimentValue.ToString("0.0000")));
                        TempRes += string.Format("{0} ({1}) {2}{3} {4}", entity.Text, entity.KeywordType, entity.SentimentPolarity, entity.SentimentResult, entity.SentimentValue.ToString("0.0000"));
                    }
                }*/
                #endregion

                #region display keyword scores if any found

                /*if (docResult.Keywords != null && docResult.Keywords.Any())
                {
                    //Console.WriteLine(Environment.NewLine + "Keywords:");
                    TempRes += Environment.NewLine + "Keywords:";
                    foreach (var keyword in docResult.Keywords)
                    {
                        //Console.WriteLine(string.Format("{0} {1}{2} {3}", keyword.Text, keyword.SentimentPolarity, keyword.SentimentResult, keyword.SentimentValue.ToString("0.0000")));
                        TempRes += string.Format("{0} {1}{2} {3}", keyword.Text, keyword.SentimentPolarity, keyword.SentimentResult, keyword.SentimentValue.ToString("0.0000"));
                    }
                }*/
                #endregion


                //display more information below if required 

                #endregion

                result = TempRes;
            }
            else
            {
                //Console.WriteLine(docResult.ErrorMessage);
                result = docResult.ErrorMessage;
            }

            //Console.ReadLine();
            return result;
        }

        protected void btnAnalysis_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/UploadedFiles/");
            string resultpath = Server.MapPath("~/ResultFiles/");
            string Filename = "";
            string Tempstr = "";

            #region Upload file
            Boolean fileOK = false;
            if (fuReview.HasFile)
            {
                Filename = fuReview.FileName;                
                string fileExtension = Path.GetExtension(Filename).ToLower();
                string[] allowedExtensions = { ".txt" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }

                if (fileOK)
                {
                    try
                    {
                        fuReview.PostedFile.SaveAs(path + Filename);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("File could not be uploaded.");
                    }
                }
            }
            #endregion

            /*if (File.Exists(resultpath + Filename))
                File.Delete(resultpath + Filename);*/ 

            #region dynamic
            int counter = 0;
            string line;

            Table tbl = tblAnalysis; //new Table();
            tbl.Width = Unit.Percentage(100);
            TableRow tr = null;
            TableCell tc = null;
            Label lbl = null;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(path + Filename); //new StreamReader(@"D:\Analysis\Temp\Abcde.txt");
            
            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    tr = new TableRow();
                    tc = new TableCell();
                    tc.ColumnSpan = 2;
                    tc.Controls.Add(new LiteralControl("<hr/>"));
                    tr.Controls.Add(tc);
                    tbl.Controls.Add(tr);                                     
                }

                tr = new TableRow();
                tc = new TableCell();
                tc.Width = Unit.Pixel(100);
                lbl = new Label();
                lbl.Text = "Review:";
                tc.Controls.Add(lbl);
                tr.Controls.Add(tc);

                tc = new TableCell();
                lbl = new Label();
                lbl.ID = "lbl" + counter;
                lbl.Text = line;
                //Tempstr += "Review: " + lbl.Text + "^";
                tc.Controls.Add(lbl);
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Height = Unit.Pixel(4);
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Width = Unit.Pixel(100);
                lbl = new Label();
                lbl.Text = "Result:"; 
                tc.Controls.Add(lbl);
                tr.Controls.Add(tc);

                tc = new TableCell();
                lbl = new Label();
                lbl.Text = GetResult(line + "\\ASC");
                Tempstr += lbl.Text + "^";
                //if ( Tempstr = 'Positive' | 'Ne
                tc.Controls.Add(lbl);
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.ColumnSpan = 2;
                tc.Controls.Add(new LiteralControl("<hr/>"));
                tr.Controls.Add(tc);
                tbl.Controls.Add(tr);
                counter++;
            }

            //this.Controls.Add(tbl); 
            file.Close();
            #endregion

            #region Result File
            if (!File.Exists(resultpath + Filename))
                File.Create(resultpath + Filename);

            Tempstr = Tempstr.Substring(0, Tempstr.Length - 1);
            string[] TempArr = Tempstr.Split('^');
            File.WriteAllLines(resultpath + Filename, TempArr);
            #endregion

            #region Delete file
            /*if (File.Exists(path + Filename))
            {
                try
                {
                    File.Delete(resultpath + Filename); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File could not be deleted.");
                }
            }*/
            #endregion
        }
    }
}
