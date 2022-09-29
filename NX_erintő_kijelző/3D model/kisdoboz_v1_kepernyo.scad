 module kistarto(l, w, h){
      rotate([90, 0, 0]) polyhedron(
               points=[[0,0,0], [l,0,0], [l,w,0], [0,w,0], [0,w,h], [l,w,h]],
              faces=[[0,1,2,3],[5,4,3,2],[0,4,5,1],[0,3,4],[5,2,1]]
              );
}

layerDepth=2;
height=80;
width=130;
//alja
cube([width, height, layerDepth]);
//bal oldala
cube([layerDepth, height, height]);

//jobb oldala
translate([128, 0, 0]) cube([layerDepth, height, height]);
//eleje
difference(){
cube([width, layerDepth, height]);
translate([10,0,65]) cube([110,layerDepth, 15]);
}

// nyomtatáshoz mellé tevés
rotate([180]){
translate([0, 100, 0]){
//elenorzeshez 90 megelelve 80 rajta
//    translate([0, 0, 80]){
    //teteje
difference(){
   translate([0, 0, 0])
    cube([width, height, layerDepth]);
    translate([10, 0, 0]) 
    cube([110, 15, layerDepth]);
}
//bal elso
    translate([2.5, 2.5, -9]) cube([5, 5, 10]);
//bal hatso
    translate([2.5, 72.5, -9]) cube([5, 5, 10]);
//jobb elso
    translate([122.5, 2.5, -9]) cube([5, 5, 10]);
//bal elso
    translate([122.5, 72.5, -9]) cube([5, 5, 10]);
}
}


//belso tamasz
//bal elos
translate([43, 17, 0]) cube([10, 10, height]);
//jobb elso
translate([83, 17, 0]) cube([10, 10, height]);
//bal hatso
translate([43, 60, 0]) cube([10, 10, height]);
//jobb hatso
translate([83, 60, 0]) cube([10, 10, height]);

//4 szélso cucc
//bal eslo
difference(){
    cube([10, 10, height]);
    translate([0, 0, 70]) cube([8, 8, 11]);
}
//bal hatso
difference(){
    translate([0, 70, 0]) cube([10, 10, height]);
    translate([0, 72, 70]) cube([8, 8, 11]);
}
//jobb elso
difference(){
    translate([120, 0, 0]) cube([10, 10, height]);
    translate([122, 0, 70]) cube([8, 8, 11]);
}
//jobb hatso
difference(){
    translate([120, 70, 0]) cube([10, 10, height]);
    translate([122, 72, 70]) cube([8, 8, 11]);
}
//elso kis biszbasz
translate([10, -10, 50]) cube([110, 12, 3]);
translate([10, -10, 50]) cube([110, layerDepth, 5]);
//haromszög alatamsz
//ball
translate([30, 0, 40]) kistarto(10,10,10);
//kozep
translate([60, 0, 40]) kistarto(10,10,10);
//jobb
translate([90, 0, 40]) kistarto(10,10,10);


//hatulja
difference(){
translate([0, 78, 0]) cube([width, layerDepth, height]);
translate([55, 78, 10]) cube([20, layerDepth, 10]);
}
 
      
