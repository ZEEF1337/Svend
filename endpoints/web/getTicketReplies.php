<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: GET");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");


if(!isset($_GET['token']) || !isset($_GET['ticketID'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_GET['token'];
$givenTicketID = $_GET['ticketID'];

$database = new Database();
$db = $database->getConnection();

$UserID = getUserIDFromToken($givenToken);

$query = "SELECT ticket_replies.CreationDate, ticket_replies.Body, u.Fornavn, u.Efternavn, ur.Navn AS Rolle FROM ticket_replies";
$query .= " INNER JOIN users AS u ON u.ID = ticket_replies.UserID";
$query .= " INNER JOIN user_roles AS ur ON ur.ID = u.Rolle";
$query .= " WHERE ticket_replies.TicketID = $givenTicketID;";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $num = $stmt->rowCount();
    $out = array();
    $out["records"] = array();
    if($num > 0){
        while ($row = $stmt->fetch(PDO::FETCH_ASSOC)){
            extract($row);
            $temp = array(
                "Fornavn" => $Fornavn,
                "Efternavn" => $Efternavn,
                "Rolle" => $Rolle,
                "Dato" => date('d/m/Y', strtotime($CreationDate)),
                "Klok" => date('H:i:s', strtotime($CreationDate)),
                "Body" => $Body,
            );
            array_push($out["records"], $temp);
        }
        $out['result'] = 1;
    }else{
        $out['result'] = 0;
        $out['message'] = "Ingen resultater fundet";
    }
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e->getMessage();
}
$json = json_encode($out);
print_r($json);
return;
?>