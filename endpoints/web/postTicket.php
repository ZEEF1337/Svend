<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."functions/propFunctions.php");
include_once ($_SERVER['DOCUMENT_ROOT']."database.inc");

if(!isset($_POST['title']) || !isset($_POST['body']) || !isset($_POST['category'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_POST['token'];
$givenTitle = $_POST['title'];
$givenBody = $_POST['body'];
$givenCategory = $_POST['category'];

$database = new Database();
$db = $database->getConnection();

$userID = getUserIDFromToken($givenToken);

$query = "INSERT INTO tickets (UserID, Kategori, Title, Body) VALUES ('$userID', '$givenCategory', '$givenTitle', '$givenBody');";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $out['result'] = 1;
    $out['message'] = "Ticket blev oprettet";
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e;
}

$json = json_encode($out);
print_r($json);
return;
?>