let url = "http://192.168.2.8:8003/"
let debug = false;


let lamp_state = false;
let lamp_auto = true;
let brightness = 255;

let background = 1;

let bug_icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-bug-fill\" viewBox=\"0 0 16 16\">\n" +
    "  <path d=\"M4.978.855a.5.5 0 1 0-.956.29l.41 1.352A4.985 4.985 0 0 0 3 6h10a4.985 4.985 0 0 0-1.432-3.503l.41-1.352a.5.5 0 1 0-.956-.29l-.291.956A4.978 4.978 0 0 0 8 1a4.979 4.979 0 0 0-2.731.811l-.29-.956z\"/>\n" +
    "  <path d=\"M13 6v1H8.5v8.975A5 5 0 0 0 13 11h.5a.5.5 0 0 1 .5.5v.5a.5.5 0 1 0 1 0v-.5a1.5 1.5 0 0 0-1.5-1.5H13V9h1.5a.5.5 0 0 0 0-1H13V7h.5A1.5 1.5 0 0 0 15 5.5V5a.5.5 0 0 0-1 0v.5a.5.5 0 0 1-.5.5H13zm-5.5 9.975V7H3V6h-.5a.5.5 0 0 1-.5-.5V5a.5.5 0 0 0-1 0v.5A1.5 1.5 0 0 0 2.5 7H3v1H1.5a.5.5 0 0 0 0 1H3v1h-.5A1.5 1.5 0 0 0 1 11.5v.5a.5.5 0 1 0 1 0v-.5a.5.5 0 0 1 .5-.5H3a5 5 0 0 0 4.5 4.975z\"/>\n" +
    "</svg>"


$("#white_brightness").change(function() {
    var rangePercent = $('#white_brightness_range').val();
    $('#white_brightness_range').on('change input', function() {
        rangePercent = $('[type="range"]').val();
        let precent  = rangePercent/2.55
        $('#white_brightness_value').html( Math.ceil(precent)+'<span></span>');
        $('#white_brightness_range, #white_brightness_value>span').css('filter', 'hue-rotate(-' + precent + 'deg)');

        $('#white_brightness_value').css({'transform': 'translateX(calc(-50% - 20px)) scale(' + (1+(rangePercent/100)) + ')', 'left': precent +'%'});
        // $('#white_brightness_value').css({'transform': 'translateX(-50%) scale(' + (1+(rangePercent/100)) + ')', 'left': rangePercent+'%'});
    });
});
$("#white_brightness_button").click(async function (){
    let debug_element = document.getElementById("white_brightness").getElementsByClassName("card__link")[0];
    await request("brightness?level="+$("#white_brightness_range").val(), debug_element)
});
$("#switch").click(async function (){
    let text = document.getElementById("switch").getElementsByClassName("card__title")[0]
    let debug_element = document.getElementById("switch").getElementsByClassName("card__link")[0];
    if (lamp_state){
        text.innerHTML = "Lamp off";
        await request("off", debug_element);
    }else {
        text.innerHTML = "Lamp on";
        await request("on", debug_element);
    }
    lamp_state = !lamp_state;
});

$("#auto").click(async function (){
    let text = document.getElementById("auto").getElementsByClassName("card__title")[0];
    let debug_element = document.getElementById("auto").getElementsByClassName("card__link")[0];
    await request("auto", debug_element);
    if (lamp_auto){
        text.innerHTML = "Manual Lamp";
    }else {
        text.innerHTML = "Automatic Lamp";
    }
    lamp_auto = !lamp_auto;
});




async function request(command, debug_element){
    let xhr = new XMLHttpRequest();
    xhr.open("GET", url+command, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            if (debug){
                console.log("response: " + xhr.responseText);
                debug_element.innerHTML =  bug_icon+"-->"+xhr.responseText;
            }
        }else {
            if (debug){
                console.log("response: " + xhr.status);
                debug_element.innerHTML =   bug_icon+"-->"+"error: " + xhr.status;
            }
        }
    };
    xhr.send();
}

async function start_sett() {
    if (debug) {
        console.log("status_run")
    }
    sett_status();
    document.getElementById("settings").getElementsByClassName("card__icon")[0].innerHTML = background;
    console.log(brightness)

    // 5 secund update
    // setInterval(sett_status, 60000);
    setInterval(sett_status, 5000);
}
async function sett_status(){
    let xhr = new XMLHttpRequest();
    xhr.open("GET", url+"status/power", true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            lamp_state = JSON.parse(xhr.responseText).power;

            let text = document.getElementById("switch").getElementsByClassName("card__title")[0]
            if (lamp_state){
                text.innerHTML = "Lamp on";
            }else {
                text.innerHTML = "Lamp off";
            }
            if (debug){
                console.log(lamp_state)
                console.log("response: " + xhr.responseText);
            }
        }else {
            if (debug){
                console.log("response: " + xhr.status);
            }
        }
    };
    xhr.send();

    let xhr1 = new XMLHttpRequest();
    xhr1.open("GET", url+"status/auto", true);
    xhr1.setRequestHeader("Content-Type", "application/json");
    xhr1.onreadystatechange = function () {
        if (xhr1.readyState === 4 && xhr1.status === 200) {
            lamp_auto = JSON.parse(xhr1.responseText).auto;
            let text = document.getElementById("auto").getElementsByClassName("card__title")[0]
            if (lamp_auto){
                text.innerHTML = "Manual Lamp";
            }else {
                text.innerHTML = "Automatic Lamp";
            }
            if (debug){
                console.log(lamp_auto);
                console.log("response: " + xhr1.responseText);
            }
        }else {
            if (debug){
                console.log("response: " + xhr1.status);
            }
        }
    };
    xhr1.send();

    let xhr2 = new XMLHttpRequest();
    xhr2.open("GET", url+"status/brightness", true);
    xhr2.setRequestHeader("Content-Type", "application/json");
    xhr2.onreadystatechange = function () {
        if (xhr2.readyState === 4 && xhr2.status === 200) {
            brightness = JSON.parse(xhr2.responseText).brightness;
            document.getElementById("white_brightness_range").value = brightness;
            let precent  = brightness/2.55
            $('#white_brightness_value').html( Math.ceil(precent)+'<span></span>');
            $('#white_brightness_range, #white_brightness_value>span').css('filter', 'hue-rotate(-' + precent + 'deg)');
            $('#white_brightness_value').css({'transform': 'translateX(calc(-50% - 20px)) scale(' + (1+(brightness/100)) + ')', 'left': precent +'%'});

            if (debug){
                console.log(brightness)
                console.log("response: " + xhr2.responseText);
            }
        }else {
            if (debug){
                console.log("response: " + xhr2.status);
            }
        }
    };
    xhr2.send();
}


$("#settings").click(function (){
    let body = document.getElementsByTagName("body")[0];
    background++;
    if (background > 4) background = 1;
    if(debug) {
        console.log(background)
    }
    switch (background){
        case 1:
            body.style.background = "#fff";
            body.style.color = "#333";
        break;
        case 2:
            body.style.background = "black";
            body.style.color = "#fff";
        break;
        case 3:
            body.style.background = "#444444";
            body.style.color = "#fff";
        break;
        case 4:
            body.style.background = "#f7a3b5";
            body.style.color = "#fff";
        break;
    }
    document.getElementById("settings").getElementsByClassName("card__icon")[0].innerHTML = background;
});