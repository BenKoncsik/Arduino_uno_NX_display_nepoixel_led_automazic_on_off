/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 24.
 * Class name: GetStatusContoller
 */
package hu.koncsik.ledcontroller.controller;

import lombok.extern.slf4j.Slf4j;
import netscape.javascript.JSObject;
import org.json.JSONObject;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController()
@Slf4j
public class GetStatusController {

    @GetMapping(value = "/status/power", produces = MediaType.APPLICATION_JSON_VALUE)
    public String powerStatus(){
        return new JSONObject().put("power", LedController.power).toString();
    }

    @GetMapping(value = "/status/auto", produces = MediaType.APPLICATION_JSON_VALUE)
    public String autoStatus(){
        return new JSONObject().put("auto", LedController.auto_led).toString();
    }
    @GetMapping(value = "/status/brightness", produces = MediaType.APPLICATION_JSON_VALUE)
    public String brightnessStatus(){
        return new JSONObject().put("brightness", LedController.brightness).toString();
    }
}
