<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");

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
$supporterToken = CheckSupporterToken($token);

if($login == 1){
    if($supporterToken['Result'] == 1){
        $out['Result'] = 1;
        $out['Message'] = "Login successful";
        $out['Fornavn'] = $names['Firstname'];
        $out['Efternavn'] = $names['Lastname'];
        $out['Token'] = $token;
        $out['Rolle'] = (INT)$supporterToken['Rolle'];
        $out['RolleNavn'] = $supporterToken['RolleNavn'];
    }else{
        $out['Result'] = 0;
        $out['Message'] = "Kun for personale";
    }
    
}else{
    $out['Result'] = 0;
    $out['Message'] = "Forkert brugernavn eller adgangskode.";
}

$json = json_encode($out);
print_r($json);
return;
?>