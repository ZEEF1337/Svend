<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."functions/propFunctions.php");

if(!isset($_POST['fornavn']) || !isset($_POST['efternavn']) || !isset($_POST['email']) || !isset($_POST['token'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_POST['token'];
$givenFornavn = $_POST['fornavn'];
$givenEfternavn = $_POST['efternavn'];
$givenEmail = $_POST['email'];

$database = new Database();
$db = $database->getConnection();

$userID = getUserIDFromToken($givenToken);

$query = "UPDATE users SET Fornavn='$givenFornavn', Efternavn='$givenEfternavn', Email='$givenEmail' WHERE ID = $userID;";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $out['result'] = 1;
    $out['message'] = "Din profil blev opdateret";
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e;
}

$json = json_encode($out);
print_r($json);
return;
?>