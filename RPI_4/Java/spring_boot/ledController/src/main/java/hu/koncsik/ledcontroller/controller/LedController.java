/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 16.
 * Class name: LedControleer
 */
package hu.koncsik.ledcontroller.controller;

import hu.koncsik.ledcontroller.service.UsbService;
import lombok.extern.slf4j.Slf4j;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController("")
@Slf4j
public class LedController {
    private UsbService usbService;

    @Autowired
    public void setUsbService(UsbService usbService) {
        this.usbService = usbService;
    }

    public static boolean auto_led = true;
    public static boolean power = false;
    public static int brightness  = 255;
    public static int[] color  ={255,255,255};
    public static int colorBrightness = 100;
    public static int autoLedOnBrightness = 11000;


    @GetMapping(value = "/on", produces = MediaType.APPLICATION_JSON_VALUE)
    public String on(){
        log.info("on");
        power = true;
        usbService.on();
        return new JSONObject().put("led", "on").toString();
    }

    @GetMapping(value = "/off", produces = MediaType.APPLICATION_JSON_VALUE)
    public String off(){
        log.info("off");
        power = false;
        usbService.off();
        return new JSONObject().put("led", "off").toString();
    }

    @GetMapping(value = "/measurement", produces = MediaType.APPLICATION_JSON_VALUE)
    public String measurement(){
        log.info("measurement");
        return new JSONObject().put("measurement", usbService.measurement()).toString();
    }

    @GetMapping(value = "/auto", produces = MediaType.APPLICATION_JSON_VALUE)
    public String autoLed(){
        auto_led  = usbService.autoLed();
        return new JSONObject().put("auto_led", auto_led).toString();
    }

    @GetMapping(value = "/brightness", produces = MediaType.APPLICATION_JSON_VALUE)
    public String settBrightness(@RequestParam int level){
        brightness = usbService.setBrightness(level);
        return new JSONObject().put("brightness", brightness).toString();
    }

    @GetMapping(value = "/color", produces = MediaType.APPLICATION_JSON_VALUE)
    public String color(@RequestParam int r,@RequestParam int g,@RequestParam int b){
        color = usbService.setColor(r,g,b);
        return new JSONObject().put("brightness", color).toString();
    }

    @GetMapping(value = "/color/brightness", produces = MediaType.APPLICATION_JSON_VALUE)
    public String color(@RequestParam int level){
        colorBrightness = usbService.setColorBrightness(level);
        return new JSONObject().put("brightness", colorBrightness).toString();
    }

    @GetMapping(value = "auto/brightness", produces = MediaType.APPLICATION_JSON_VALUE)
    public String autoLedBrightness(@RequestParam int level){
        autoLedOnBrightness = usbService.setOnAutoLevel(level);
        return new JSONObject().put("brightness", autoLedOnBrightness).toString();
    }

}
