/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 16.
 * Class name: LedControleer
 */
package hu.koncsik.ledcontroller.controller;

import hu.koncsik.ledcontroller.service.LedService;
import hu.koncsik.ledcontroller.service.UsbService;
import jdk.security.jarsigner.JarSigner;
import lombok.extern.slf4j.Slf4j;
import netscape.javascript.JSObject;
import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController("")
@Slf4j
public class LedController {
    private LedService ledService;

    @Autowired
    public void setLedService(LedService ledService) {
        this.ledService = ledService;
    }
    private UsbService usbService;

    @Autowired
    public void setUsbService(UsbService usbService) {
        this.usbService = usbService;
    }

    @GetMapping(value = "/on", produces = MediaType.APPLICATION_JSON_VALUE)
    public String on(){
        log.info("on");
        usbService.on();
        return new JSONObject().put("led", "on").toString();
    }

    @GetMapping(value = "/off", produces = MediaType.APPLICATION_JSON_VALUE)
    public String off(){
        log.info("off");
        usbService.off();
        return new JSONObject().put("led", "off").toString();
    }

    @GetMapping(value = "/measurement", produces = MediaType.APPLICATION_JSON_VALUE)
    public String measurement(){
        log.info("measurement");
        return new JSONObject().put("measurement", usbService.measurement()).toString();
    }

    @GetMapping(value = "/meres", produces = MediaType.APPLICATION_JSON_VALUE)
    public String measurement_1(){
        log.info("measurement");
        return new JSONObject().put("measurement", usbService.measurement()).toString();
    }



    @GetMapping(value = "/auto", produces = MediaType.APPLICATION_JSON_VALUE)
    public String autoLed(){
        return new JSONObject().put("auto_led", usbService.autoLed()).toString();
    }

    @GetMapping(value = "/feny", produces = MediaType.APPLICATION_JSON_VALUE)
    public String settBrightness(@RequestParam int level){
        return new JSONObject().put("brightness", usbService.setBrightness(level)).toString();
    }
}
