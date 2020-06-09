<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."functions/propFunctions.php");
include_once ($_SERVER['DOCUMENT_ROOT']."database.inc");

if(!isset($_POST['fornavn']) || !isset($_POST['efternavn']) || !isset($_POST['email']) || !isset($_POST['brugernavn']) || !isset($_POST['password'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenFornavn = $_POST['fornavn'];
$givenEfternavn = $_POST['efternavn'];
$givenEmail = $_POST['email'];
$givenBrugernavn = $_POST['brugernavn'];
$givenPassword = $_POST['password'];


if(emailExists($givenEmail)){
    $out['result'] = 0;
    $out['message'] = "Email adressen er allerede i brug";
    $json = json_encode($out);
    print_r($json);
    return;
}else if(userExists($givenBrugernavn)){
    $out['result'] = 0;
    $out['message'] = "Brugernavnet er allerede i brug";
    $json = json_encode($out);
    print_r($json);
    return;
}

$Salt = generateRandomSalt(32);
$Password = hash('sha512', hash('sha512', $Salt).$givenPassword);


$database = new Database();
$db = $database->getConnection();

$query = "INSERT INTO users (Fornavn, Efternavn, Email, Brugernavn, Password, Salt) VALUES ('$givenFornavn', '$givenEfternavn', '$givenEmail', '$givenBrugernavn', '$Password', '$Salt');";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $out['result'] = 1;
    $out['message'] = "Kontoen blev oprettet";
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e;
}

$json = json_encode($out);
print_r($json);
return;

?>