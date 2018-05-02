function Validate(txt, txt1) {
    var txtbx = document.getElementById(txt);
    var txtbx1 = document.getElementById(txt1);
    var dis = Math.round(txtbx.value.replace(",", "."));
    var pre = Math.round(txtbx1.value.replace(",", "."));
    
    if (dis > pre) {
        alert("Dispense quantity cannot be greater than prescribed quantity");
        txtbx.value = '';
    }


}
function fnSetBMI(txtw, txth, txtbmi) {

    var weight = document.getElementById(txtw).value.replace(",", ".");
    var height = document.getElementById(txth).value.replace(",", ".");
    var txtbmi= document.getElementById(txtbmi);
    if (weight > 0 && height > 0) {
        var finalBmi = weight / (height / 100 * height / 100)
        txtbmi.value = finalBmi
    }
}

function CalculateTotalDailyDose(txtdose, txtdur, ddfr,txtpre) {
    var dose = document.getElementById(txtdose).value.replace(",", ".");
    var duration = document.getElementById(txtdur).value.replace(",", ".");
    var selText = document.getElementById(ddfr).value.replace(",", ".");
   
    var multiplier = 0;
    if (selText == 'OD') {
        multiplier = 1;

    }
    else if (selText == 'BD')
    {
        multiplier = 2;
    }
    else if (selText == 'TID')
    {
        multiplier = 3;
    }
    else if (selText == 'QID') {
        multiplier = 4;
    }


    if (selText != "0" && dose != "" && duration != "") {

        var TotalDose = dose * duration * multiplier;
        
        document.getElementById(txtpre).value = TotalDose;
    }
    else {
        document.getElementById(txtpre).value = "";
    }
}