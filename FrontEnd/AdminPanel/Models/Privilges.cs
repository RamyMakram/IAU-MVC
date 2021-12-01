using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models
{
	public class Privilges
	{
		public static List<string> GlobalPrivilges = new List<string>()
		{
			"Home".ToUpper(),
			"Email".ToUpper(),
			"SendedRequests".ToUpper(),
			"BASE",
			""
		};
		public static List<PagesPriviliges> PagesPriviliges = new List<PagesPriviliges>() {
			new PagesPriviliges()
			{
				Name = "Users",
				path = "User"
			},
			new PagesPriviliges()
			{
				Name = "Applicant Types",
				path = "Applicanttype"
			},
			new PagesPriviliges()
			{
				Name = "Permissions",
				path = "Jobs"
			},new PagesPriviliges()
			{
				Name = "Statistics",
				path = "Report"
			},
			new PagesPriviliges()
			{
				Name = "Adminstrative Units",
				path = "Units"
			},
			new PagesPriviliges()
			{
				Name = "Service Bank",
				path = "ServicesBank,SubServices,EForms"
			},
			new PagesPriviliges()
			{
				Name = "Administrative Levels Settings",
				path = "Levels"
			},
			new PagesPriviliges()
			{
				Name = "Site encoding settings",
				path = "UnitLocation"
			},
			new PagesPriviliges()
			{
				Name = "Main Services Settings",
				path = "ServiceTypes"
			},
			new PagesPriviliges()
			{
				Name = "Request Types",
				path = "RequestType"
			},
			new PagesPriviliges()
			{
				Name = "General Settings",
				path = "GlobalSettings"
			},
			new PagesPriviliges()
			{
				Name = "Delayed Transactions Alert",
				path = "DelayedTrasaction"
			}
		};
	}
}