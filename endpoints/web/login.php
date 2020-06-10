<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."functions/propFunctions.php");

if(!isset($_POST['brugernavn']) || !isset($_POST['password'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}
$givenBrugernavn = $_POST['brugernavn'];
$givenPassword = $_POST['password'];

if(!userExists($givenBrugernavn)){
    $out['result'] = 0;
    $out['message'] = "Brugeren blev ikke fundet";
    $json = json_encode($out);
    print_r($json);
    return;
}

$login = verifyPass($givenBrugernavn, $givenPassword);
$names = getFirstLastNameFromUsername($givenBrugernavn);
$token = generateToken($givenBrugernavn);

if($login == 1){
    $out['result'] = 1;
    $out['message'] = "Login successful";
    $out['firstname'] = $names['Firstname'];
    $out['lastname'] = $names['Lastname'];
    $out['token'] = $token;
}else{
    $out['result'] = 0;
    $out['message'] = "Forkert brugernavn eller adgangskode.";
}

$json = json_encode($out);
print_r($json);
return;
?>