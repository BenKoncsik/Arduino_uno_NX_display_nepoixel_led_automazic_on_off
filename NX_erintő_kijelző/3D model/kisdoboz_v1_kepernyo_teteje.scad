 module kistarto(l, w, h){
      rotate([90, 0, 0]) polyhedron(
               points=[[0,0,0], [l,0,0], [l,w,0], [0,w,0], [0,w,h], [l,w,h]],
              faces=[[0,1,2,3],[5,4,3,2],[0,4,5,1],[0,3,4],[5,2,1]]
              );
}
layerDepth=2;
height=80;
width=130;
// nyomtatáshoz mellé tevés
rotate([180]){
translate([0, -80, 0]){
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