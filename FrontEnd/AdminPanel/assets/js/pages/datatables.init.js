/*
Template Name: Skote - Responsive Bootstrap 4 Admin Dashboard
Author: Themesbrand
Version: 2.0
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Datatables Js File
*/

$(document).ready(function() {
    $('#datatable').DataTable();
	let cook = document.cookie.split(';');
	let language = 'en';
	cook.forEach(i => {
		let keyandval = i.split('=');
		if (keyandval[0].trim().replace(' ', '') == "lang") {
			language = i.split("lang=")[1].replace(' ', '')
		}
		else if (keyandval[0].trim().replace(' ', '') == "u") {
			User = i.split("u=")[1].replace(' ', '')
			if (User == "") {
				document.cookie = "";
				location.href = "/"
			}
		}
	})
    //Buttons examples
    var table = $('#datatable-buttons').DataTable({
        lengthChange: false,
        buttons: [/*'excel', 'pdf'*/],
        language: {
			url: language == "ar" ? '/assets/DataTableTrans/ar.json' :'/assets/DataTableTrans/en.json'
		},
		initComplete: function (settings, json) {
			//alert('DataTables has finished its initialisation.');
			getLanguage()
		}
	});


    table.buttons().container()
		.appendTo('#datatable-buttons_wrapper .col-md-6:eq(0)');
} );