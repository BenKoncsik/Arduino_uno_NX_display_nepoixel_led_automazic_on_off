let url = "http://192.168.2.8:8003/"
let debug = false;


let lamp_state = false;
let lamp_auto = true;
let brightness = 255;
let auto_led_brightness = 11000;
let openSetting = false;

let auto_update = true;
let background = 1;
let ip_list = ["http://192.168.2.8:8003/", "http://localhost:8080/"];

let bug_icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-bug-fill\" viewBox=\"0 0 16 16\">\n" +
    "  <path d=\"M4.978.855a.5.5 0 1 0-.956.29l.41 1.352A4.985 4.985 0 0 0 3 6h10a4.985 4.985 0 0 0-1.432-3.503l.41-1.352a.5.5 0 1 0-.956-.29l-.291.956A4.978 4.978 0 0 0 8 1a4.979 4.979 0 0 0-2.731.811l-.29-.956z\"/>\n" +
    "  <path d=\"M13 6v1H8.5v8.975A5 5 0 0 0 13 11h.5a.5.5 0 0 1 .5.5v.5a.5.5 0 1 0 1 0v-.5a1.5 1.5 0 0 0-1.5-1.5H13V9h1.5a.5.5 0 0 0 0-1H13V7h.5A1.5 1.5 0 0 0 15 5.5V5a.5.5 0 0 0-1 0v.5a.5.5 0 0 1-.5.5H13zm-5.5 9.975V7H3V6h-.5a.5.5 0 0 1-.5-.5V5a.5.5 0 0 0-1 0v.5A1.5 1.5 0 0 0 2.5 7H3v1H1.5a.5.5 0 0 0 0 1H3v1h-.5A1.5 1.5 0 0 0 1 11.5v.5a.5.5 0 1 0 1 0v-.5a.5.5 0 0 1 .5-.5H3a5 5 0 0 0 4.5 4.975z\"/>\n" +
    "</svg>"

async function start_set() {
    if (debug) {
        console.log("status_run")
    }
    cookie();
    set_ips();
    set_background();
    set_status();
    if (debug) console.log("brightness: " + brightness)
    document.getElementById("sett_ip").getElementsByClassName("card__icon")[0].innerHTML = url;
    // 10 secund update
    // setInterval(sett_status, 60000);
    setInterval(set_status, 10000);
    setInterval(stop_auto_update, 10);
    // stop_auto_update();
}

async function stop_auto_update(){
    if ($('.card:hover').length > 0) {
        auto_update = false;
    } else {
        auto_update = true;
    }
    if(debug) console.log("auto_update "+auto_update);
}


$("#white_brightness").change(function() {
    let rangePercent = $('#white_brightness_range').val();
    $('#white_brightness_range').on('change input', function() {
        rangePercent = $('#white_brightness_range').val();
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

$("#auto_led_brightness").change(function() {
    let rangePercent = $('#auto_led_brightness_range').val();
    $('#auto_led_brightness_range').on('change input', function() {
        rangePercent = $('#auto_led_brightness_range').val();
        let precent  = rangePercent/2.55
        $('#auto_led_brightness_value').html( Math.ceil(precent)+'<span></span>');
        $('#auto_led_brightness_range, #auto_led_brightness_value>span').css('filter', 'hue-rotate(-' + precent + 'deg)');
        $('#auto_led_brightness_value').css({'transform': 'translateX(calc(-50% - 20px)) scale(' + (1+(rangePercent/100)) + ')', 'left': precent +'%'});

    });
});
$("#auto_led_brightness_button").click(async function (){
    let debug_element = document.getElementById("auto_led_brightness").getElementsByClassName("card__link")[0];
    await request("auto/brightness?level="+$("#auto_led_brightness_range").val(), debug_element)
});

$("#switch").click(async function (){
    let text = document.getElementById("switch").getElementsByClassName("card__title")[0]
    let debug_element = document.getElementById("switch").getElementsByClassName("card__link")[0];
    if (lamp_state){
        text.innerHTML = "Lamp off";
        request("off", debug_element);
    }else {
        text.innerHTML = "Lamp on";
        request("on", debug_element);
    }
    lamp_state = !lamp_state;
});

$("#auto").click(async function (){
    let text = document.getElementById("auto").getElementsByClassName("card__title")[0];
    let debug_element = document.getElementById("auto").getElementsByClassName("card__link")[0];
    await request('auto', debug_element)
    if (lamp_auto){
        text.innerHTML = "Manual Lamp";
        document.getElementById("auto_led_brightness").style.display = "none";
    }else {
        text.innerHTML = "Automatic Lamp";
        document.getElementById("auto_led_brightness").style.display = "block";
    }
    lamp_auto = !lamp_auto;

});

$("#set_ip_a").click(async function (){
    let new_ip = document.getElementById("sett_ip_list").value;
    url  = new_ip;
    setting_ip_save_cookie();
    document.getElementById("sett_ip").getElementsByClassName("card__icon")[0].innerHTML = url;
    let is_ip=false;
    ip_list.forEach(function (ip){
        if (ip === new_ip) is_ip = true;
    });
    if (!is_ip){
        ip_list.push(new_ip);
    }
    let ip_string = "";
    ip_list.forEach(function (ip){
        if(debug) console.log("ip: " + ip)
        if (debug){
            console.log("undefined: " + (ip !== 'undefined') + " 192.168.2.8=" +  (ip !== "http://192.168.2.8:8003/") +
                " loaclhost= " + (ip !== "http://localhost:8080/"));
        }
        if (ip !== 'undefined' && ip !== "http://192.168.2.8:8003/" && ip !== "http://localhost:8080/") {
            if (debug)console.log("hozzadava");
            ip_string += ip + ";";
        }
    })
    if(debug) console.log("ip list to string: " + ip_string);
    cookiemonster.set("ips", ip_string, 30);
    set_ips();
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
                return xhr.responseText;
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


async function set_status(){
    if (auto_update) {
        let xhr = new XMLHttpRequest();
        xhr.open("GET", url + "status", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                let response = JSON.parse(xhr.responseText)
                lamp_state = response.power;
                let text = document.getElementById("switch").getElementsByClassName("card__title")[0]
                if (lamp_state) {
                    text.innerHTML = "Lamp on";
                } else {
                    text.innerHTML = "Lamp off";
                }
                lamp_auto = response.auto;
                text = document.getElementById("auto").getElementsByClassName("card__title")[0]
                if (lamp_auto) {
                    text.innerHTML = "Automatic Lamp";
                    document.getElementById("auto_led_brightness").style.display = "block";
                } else {
                    text.innerHTML = "Manual Lamp";
                    document.getElementById("auto_led_brightness").style.display = "none";
                }
                brightness = response.brightness;
                document.getElementById("white_brightness_range").value = brightness;
                let precent = brightness / 2.55;
                $('#white_brightness_value').html(Math.ceil(precent) + '<span></span>');
                $('#white_brightness_range, #white_brightness_value>span').css('filter', 'hue-rotate(-' + precent + 'deg)');
                $('#white_brightness_value').css({
                    'transform': 'translateX(calc(-50% - 20px)) scale(' + (1 + (brightness / 100)) + ')',
                    'left': precent + '%'
                });

                auto_led_brightness = response.auto_led_brightness;
                rangePercent = document.getElementById("auto_led_brightness_range").value = auto_led_brightness
                precent = rangePercent / 2.55
                $('#auto_led_brightness_value').html(Math.ceil(precent) + '<span></span>');
                $('#auto_led_brightness_range, #auto_led_brightness_value>span').css('filter', 'hue-rotate(-' + precent + 'deg)');
                $('#auto_led_brightness_value').css({
                    'transform': 'translateX(calc(-50% - 20px)) scale(' + (1 + (rangePercent / 100)) + ')',
                    'left': precent + '%'
                });
                if (debug) {
                    console.log(lamp_state)
                    console.log("response: " + xhr.responseText);
                }
            } else {
                if (debug) {
                    console.log("response: " + xhr.status);
                }
            }
        };
        xhr.send();
    }
}


$("#background_setting").click(function (){
    background++;
    if (background > 4) background = 1;
    set_background();
});

function cookie(){
    try {
        background = parseInt(cookiemonster.get("background"));
    }catch (e){
        if (debug) console.log("no cookie");
    }

    try {
        url = cookiemonster.get("set_url");
    }catch (e){
        if (debug) console.log("no cookie");
    }


    try {
        let cookie_string =  cookiemonster.get("ips");
        if(debug) console.log("ip list to string: " + cookie_string);
        cookie_string.split(';').forEach(function (ip){
           if(debug) console.log("ip: " + ip)
            if (debug){
                console.log("undefined: " + (ip !== 'undefined') + " 192.168.2.8=" +  (ip !== "http://192.168.2.8:8003/") +
                " loaclhost= " + (ip !== "http://localhost:8080/"));
            }
            if (ip !== 'undefined' && ip !== "http://192.168.2.8:8003/" && ip !== "http://localhost:8080/") {
                if (debug)console.log("hozzadava");
                ip_list.push(ip);
            }
        });
    }catch (e){
        if (debug) console.log("no cookie");
    }

}

function set_background(){
    let body = document.getElementsByTagName("body")[0];
    let h1 = document.getElementsByTagName("h1")[0];
    if(debug) {
        console.log("theme: " + background)
    }
    switch (background){
        case 1:
            body.style.background = "#fff";
            body.style.color = "#333";
            h1.style.color = "#333"
            break;
        case 2:
            body.style.background = "black";
            body.style.color = "#fff";
            h1.style.color = "white"
            break;
        case 3:
            body.style.background = "#444444";
            body.style.color = "#fff";
            h1.style.color = "white"
            break;
        case 4:
            body.style.background = "#f7a3b5";
            body.style.color = "#fff";
            h1.style.color = "white"
            break;
    }
    cookiemonster.set("background", background, 30)
    document.getElementById("background_setting").getElementsByClassName("card__icon")[0].innerHTML = background;
}


$("#settings").click(function (){

    if (openSetting) {
        $("#setting_menu").removeClass("open");
        $("#setting_menu").addClass("close");
        document.getElementsByTagName("body")[0].style.overflow = "auto";
    }else {
        $("#setting_menu").removeClass("close");
        $("#setting_menu").addClass("open");
        document.getElementsByTagName("body")[0].style.overflow = "hidden";
    }

    openSetting = !openSetting;
});

$("#exit_setting_menu").click(function (){
    $("#setting_menu").removeClass("open");
    $("#setting_menu").addClass("close");
    document.getElementsByTagName("body")[0].style.overflow = "auto";
    openSetting = false;
})

$("#debug_mode").click(function debugMode(){
    let debug_div = document.getElementById("debug_mode").getElementsByClassName("card__icon")[0];
    if (debug){
        debug_div.innerHTML = "Off";
    }else {
        debug_div.innerHTML = "On";
    }
    debug = !debug;
});


async function set_ips(){
    let datalist =  document.getElementById("ip_address");
    ip_list.forEach(function (ip){
            let option = document.createElement('option');
            option.value = ip;
            datalist.appendChild(option);
    });
}

async function setting_ip_save_cookie(){
    cookiemonster.set("set_url", url);
}
