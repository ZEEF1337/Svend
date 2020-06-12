<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: POST");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");

if(!isset($_POST['token']) || !isset($_POST['ticketID']) || !isset($_POST['userID'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_POST['token'];
$givenTicketID = $_POST['ticketID'];
$givenUserID = $_POST['userID'];

$supporterToken = CheckSupporterToken($givenToken);
if($supporterToken['Result'] != 1){
    $out['result'] = 0;
    $out['message'] = "Kun for personale";
    $json = json_encode($out);
    print_r($json);
    return;
}

$database = new Database();
$db = $database->getConnection();


$query = "DELETE FROM ticket_assigned WHERE TicketID = $givenTicketID && UserID = $givenUserID;";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $out['result'] = 1;
    $out['message'] = "Brugeren blev fjernet fra ticket";
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e;
}

$json = json_encode($out);
print_r($json);
return;
?>