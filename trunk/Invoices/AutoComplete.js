

function GetCurrencyID(sCURRENCY_ID)
{
	var sCURRENCY = '';
	var fldCURRENCY = document.getElementById(sCURRENCY_ID);
	if ( fldCURRENCY != null )
		sCURRENCY = fldCURRENCY.options[fldCURRENCY.selectedIndex].value;
	return sCURRENCY;
}

function ItemNameChanged(sCURRENCY_ID, fldINVOICE_NAME)
{
	var fldAjaxErrors = document.getElementById('AjaxErrors');
	fldAjaxErrors.innerHTML = '';
	var userContext = fldINVOICE_NAME.id.replace('INVOICE_NAME', '');
	
	var fldPREVIOUS_NAME = document.getElementById(userContext + 'PREVIOUS_NAME');
	if ( fldPREVIOUS_NAME.value != fldINVOICE_NAME.value )
	{
		if ( fldINVOICE_NAME.value.length > 0 )
			CRM.InvoiceManagement.Invoices.AutoComplete.GetInvoiceByName(GetCurrencyID(sCURRENCY_ID), fldINVOICE_NAME.value, ItemChanged_OnSucceededWithContext, ItemChanged_OnFailed, userContext);
	}
}

function ItemChanged_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID                  = result.ID                 ;
		var sNAME                = result.NAME               ;
		var dAMOUNT_DUE          = result.AMOUNT_DUE         ;
		var dAMOUNT_DUE_USDOLLAR = result.AMOUNT_DUE_USDOLLAR;
		
		var fldAjaxErrors      = document.getElementById('AjaxErrors');
		var fldINVOICE_ID      = document.getElementById(userContext + 'INVOICE_ID'     );
		var fldINVOICE_NAME    = document.getElementById(userContext + 'INVOICE_NAME'   );
		var fldAMOUNT          = document.getElementById(userContext + 'AMOUNT'         );
		var fldAMOUNT_USDOLLAR = document.getElementById(userContext + 'AMOUNT_USDOLLAR');
		if ( fldINVOICE_ID      != null ) fldINVOICE_ID.value      = sID                 ;
		if ( fldINVOICE_NAME    != null ) fldINVOICE_NAME.value    = sNAME               ;
		if ( fldAMOUNT          != null ) fldAMOUNT.value          = dAMOUNT_DUE.localeFormat('c');
		if ( fldAMOUNT_USDOLLAR != null ) fldAMOUNT_USDOLLAR.value = dAMOUNT_DUE_USDOLLAR;

		var fldPREVIOUS_NAME = document.getElementById(userContext + 'PREVIOUS_NAME');
		if ( fldPREVIOUS_NAME != null ) fldPREVIOUS_NAME.value = sNAME;
	}
	else
	{
		alert('result from AutoComplete service is null');
	}
}
function ItemChanged_OnFailed(error, userContext)
{
	var fldAjaxErrors = document.getElementById('AjaxErrors');
	fldAjaxErrors.innerHTML = 'Service Error: ' + error.get_message();
}

if ( typeof(Sys) !== 'undefined' )
	Sys.Application.notifyScriptLoaded();

