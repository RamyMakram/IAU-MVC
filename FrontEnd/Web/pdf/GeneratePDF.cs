using IAU.DTO.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.pdf
{
	public class GeneratePDF
	{
		Document _doc = new Document();
		iTextSharp.text.Font font;
		PdfPTable pdfTable = new PdfPTable(1);
		MemoryStream memoryStream = new MemoryStream();
		PdfPCell pdfCell;
		string logPath = HttpContext.Current.Server.MapPath("~/Design/img/");
		private PdfPTable logo()
		{

			int maxcolw = 5;
			PdfPTable pdfP = new PdfPTable(maxcolw);
			iTextSharp.text.Image imgg = iTextSharp.text.Image.GetInstance(Path.Combine(logPath + "MousLogo.png"));
			imgg.ScaleToFit(140f, 120f);
			imgg.SpacingBefore = 10f;
			imgg.SpacingAfter = 1f;
			pdfCell = new PdfPCell(imgg, true);
			pdfCell.Colspan = 1;
			pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
			pdfCell.Border = 0;
			pdfCell.ExtraParagraphSpace = 0;
			pdfP.AddCell(pdfCell);
			for (int i = 0; i < 3; i++)
			{
				pdfCell = new PdfPCell(new Phrase(""));
				pdfCell.Colspan = 1;
				pdfCell.Border = 0;
				pdfP.AddCell(pdfCell);
			}
			imgg = iTextSharp.text.Image.GetInstance(Path.Combine(logPath + "VisionLogo.png"));
			imgg.ScaleToFit(140f, 120f);
			imgg.SpacingBefore = 10f;
			imgg.SpacingAfter = 1f;
			pdfCell = new PdfPCell(imgg, true);
			pdfCell.Colspan = 1;
			pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
			pdfCell.Border = 0;
			pdfCell.ExtraParagraphSpace = 0;
			pdfP.AddCell(pdfCell);
			pdfP.CompleteRow();
			return pdfP;
		}
		public string GenratePDF(ApplicantRequest_Data_DTO request_Data)
		{
			try
			{
				if (request_Data == null)
					return null;

				BaseFont customfont = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Design/webfonts/") + "FrutigerLTArabic-65Bold.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);

				_doc.SetPageSize(PageSize.A4);
				_doc.SetMargins(15f, 15f, 20f, 15f);
				pdfTable.WidthPercentage = 100;
				MemoryStream stream = new MemoryStream();
				PdfWriter pdfWrite = PdfWriter.GetInstance(_doc, stream);
				_doc.Open();
				var temptable = new PdfPTable(2);
				///////////////////////////Logo////////////////////////////
				pdfCell = new PdfPCell(this.logo());
				pdfCell.Colspan = 1;
				pdfCell.Border = 0;
				pdfTable.AddCell(pdfCell);
				////////////////////////////////////////////////////////
				////////////////////////Style///////////////////////////
				var boldfont = new Font(customfont, 14, 0);
				var HeadColor = new iTextSharp.text.BaseColor(204, 204, 204);
				font = new Font(customfont, 12);
				var titleFont = new Font(customfont, 12, 0);
				var minheigth_title = 30;
				var minheigth_val = 20;

				////////////////////////////////////////////////////////
				////////////////////////////Header/////////////////////
				pdfCell = new PdfPCell(new Phrase("", boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				pdfCell.MinimumHeight = 20;
				pdfCell.Border = 0;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();
				pdfCell = new PdfPCell(new Phrase("Summary OF Request:", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				pdfCell.MinimumHeight = 50;
				pdfCell.Border = 0;
				pdfTable.AddCell(pdfCell);
				//////////////////////////////////////////////////////////////////////
				/////////////////////////////////MainRequest///////////////////////////////
				#region MainRequestData
				//pdfCell = new PdfPCell(new Phrase("Main Request Data", boldfont));
				//pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				//pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				//pdfCell.BorderColor = HeadColor;
				//pdfCell.MinimumHeight = minheigth_title;
				//pdfTable.AddCell(pdfCell);
				//pdfTable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<ServiceType>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Service type: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Service_Type_Name.Replace("\n", ""), font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<RequestType>>>>>>>>>>>
				pdfCell = new PdfPCell(new Phrase("Request type: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Request_Type_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();
				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();
				#endregion
				/////////////////////////////////////////////////////////////////////////////////
				//////////////////////////////////////Personal Data/////////////////////////////
				#region PersonalData
				pdfCell = new PdfPCell(new Phrase("Personal Data".ToUpper(), boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_title;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();

				pdfCell = new PdfPCell(new Phrase("General Information".ToUpper(), boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_title;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();

				temptable = new PdfPTable(2);
				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				pdfCell = new PdfPCell(new Phrase("IAU Affiliated: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Affiliated == 1 ? "Yes" : "No", font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);
				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				if (request_Data.Affiliated == 1)
				{
					pdfCell = new PdfPCell(new Phrase("Affilate Number: ", titleFont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					pdfCell = new PdfPCell(new Phrase(request_Data.IAUID, font));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					temptable.CompleteRow();
				}
				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Applicant type: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Applicant_Type_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Name>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("First Name: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.First_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfCell = new PdfPCell(new Phrase("Middle Name: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Middle_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfCell = new PdfPCell(new Phrase("Last Name: ".ToUpper(), titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Last_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();
				#endregion
				/////////////////////////////////////////////////////////////////////////////////
				//////////////////////////////////////Nationality/////////////////////////////
				#region PersonalData
				pdfCell = new PdfPCell(new Phrase("Nationality".ToUpper(), boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_title;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();

				temptable = new PdfPTable(2);
				pdfCell = new PdfPCell(new Phrase("Nationality: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Nationality_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Country Of Residence: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Country_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("ID Document: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.ID_Document_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("ID Number: " + request_Data.Document_Number, titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Document_Number, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();


				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Address Information>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				pdfCell = new PdfPCell(new Phrase("ADDRESS INFORMATION".ToUpper(), boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_title;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();

				temptable = new PdfPTable(2);
				pdfCell = new PdfPCell(new Phrase("City: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.City_Country_1, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfCell = new PdfPCell(new Phrase("Region: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(new Chunk(request_Data.Region, font)));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Country: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Country_Name, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();

				pdfCell = new PdfPCell(new Phrase("Postal Code: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.postal, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();
				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();


				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Address Information>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				pdfCell = new PdfPCell(new Phrase("CONTACT INFORMATION".ToUpper(), boldfont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				//pdfCell.BackgroundColor = HeadColor;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_title;
				pdfTable.AddCell(pdfCell);
				pdfTable.CompleteRow();

				temptable = new PdfPTable(2);
				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Email Address: ", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Email, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();


				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Affliiatde Number>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				pdfCell = new PdfPCell(new Phrase("Mobile Number:", titleFont));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				pdfCell = new PdfPCell(new Phrase(request_Data.Mobile, font));
				pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
				pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
				pdfCell.Border = 0;
				pdfCell.MinimumHeight = minheigth_val;
				temptable.AddCell(pdfCell);

				temptable.CompleteRow();
				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();
				/////////////////////////////////////////////////////////////////////////



				#endregion
				/////////////////////////////////////Attachment/////////////////////////
				#region Attachments
				if (request_Data.file_names != null && request_Data.file_names.Count != 0)
				{
					pdfCell = new PdfPCell(new Phrase("Attachment".ToUpper(), boldfont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
					//pdfCell.BackgroundColor = HeadColor;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_title;
					pdfTable.AddCell(pdfCell);
					pdfTable.CompleteRow();

					var name = string.Join(",    ", request_Data.file_names);
					pdfCell = new PdfPCell(new Phrase(name, font));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					pdfCell.Padding = 25;
					pdfTable.AddCell(pdfCell);
					pdfTable.CompleteRow();
				}
				#endregion
				/////////////////////////////////////////////////////////////////////////
				//////////////////////////////////Document Data/////////////////////////////
				#region DocumentData
				temptable = new PdfPTable(2);

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Provider of academic service>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				if (request_Data.provider != null || request_Data.Main_Services_ID != null || request_Data.Sub_Services_ID != null)
				{
					pdfCell = new PdfPCell(new Phrase("REQUEST DATA".ToUpper(), boldfont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
					//pdfCell.BackgroundColor = HeadColor;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_title;
					pdfTable.AddCell(pdfCell);
					pdfTable.CompleteRow();
				}
				if (request_Data.provider != null)
				{
					pdfCell = new PdfPCell(new Phrase("Provider of Service: ", titleFont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					pdfCell = new PdfPCell(new Phrase(request_Data.provider_Name, font));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					temptable.CompleteRow();
				}
				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Main service>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				if (request_Data.Main_Services_ID != null)
				{
					pdfCell = new PdfPCell(new Phrase("Main Service: ", titleFont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					pdfCell = new PdfPCell(new Phrase(request_Data.Main_Services_Name, font));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);

					temptable.CompleteRow();
				}

				//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Sub service>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

				if (request_Data.Sub_Services_ID != null)
				{
					pdfCell = new PdfPCell(new Phrase("Sub Service: ", titleFont));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);


					pdfCell = new PdfPCell(new Phrase(request_Data.Sub_Services_Name, font));
					pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
					pdfCell.VerticalAlignment = Element.ALIGN_LEFT;
					pdfCell.Border = 0;
					pdfCell.MinimumHeight = minheigth_val;
					temptable.AddCell(pdfCell);
					temptable.CompleteRow();
				}

				pdfTable.AddCell(new PdfPCell(temptable) { Border = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 10, PaddingLeft = 10 });
				pdfTable.CompleteRow();
				#endregion

				_doc.Add(pdfTable);
				_doc.Close();
				var xx = stream.ToArray();
				return Convert.ToBase64String(xx);
			}
			catch (Exception ex)
			{
				return ex.StackTrace;
			}
		}
	}
}