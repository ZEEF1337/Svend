<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");

if(!isset($_POST['oldPassword']) || !isset($_POST['password']) || !isset($_POST['token'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_POST['token'];
$givenOldPass = $_POST['oldPassword'];
$givenNewPass = $_POST['password'];


$Brugernavn = getUsernameFromToken($givenToken);
$verifyOldPass = verifyPass($Brugernavn, $givenOldPass);

if($verifyOldPass == 1){

    $salt = generateRandomSalt(32);
    $password = hash('sha512', hash('sha512', $salt).$givenNewPass);

    $database = new Database();
    $db = $database->getConnection();

    $query = "UPDATE users SET Password='$password', Salt='$salt' WHERE AuthToken='$givenToken';";
    $stmt = $db->prepare($query);
    
    try{
        $stmt->execute();
        $out['result'] = 1;
        $out['message'] = "Din adgangskode blev opdateret";
    } catch(PDOException $e){
        $out['result'] = 0;
        $out['message'] = $e;
    }
}else{
    $out['result'] = 0;
    $out['message'] = "Forkert adgangskode.";
}



$json = json_encode($out);
print_r($json);
return;
?>