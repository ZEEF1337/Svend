<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");

if(!isset($_POST['body']) || !isset($_POST['token']) || !isset($_POST['ticketID'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_POST['token'];
$givenBody = $_POST['body'];
$giventicketID = $_POST['ticketID'];

$database = new Database();
$db = $database->getConnection();

$userID = getUserIDFromToken($givenToken);

$query = "INSERT INTO ticket_replies (UserID, TicketID, Body) VALUES ('$userID', '$giventicketID', '$givenBody');";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $out['result'] = 1;
    $out['message'] = "Dit svar blev modtaget";
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e;
}

$json = json_encode($out);
print_r($json);
return;
?>