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

$query = "SELECT tickets.CreationDate, tickets.Title, TC.Navn AS Kategori, TS.Navn AS Status, tickets.Body, u.Fornavn, u.Efternavn, ur.Navn AS Rolle FROM tickets";
$query .= " INNER JOIN ticket_status AS TS ON tickets.`Status` = TS.ID";
$query .= " INNER JOIN ticket_categories AS TC ON tickets.Kategori = TC.ID";
$query .= " INNER JOIN users AS u ON u.ID = tickets.UserID";
$query .= " INNER JOIN user_roles AS ur ON ur.ID = u.Rolle";
$query .= " WHERE tickets.ID = $givenTicketID;";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $num = $stmt->rowCount();
    if($num > 0){
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        extract($row);
        $out['result'] = 1;
        $out['Dato'] = date('d/m/Y', strtotime($CreationDate));
        $out['Klok'] = date('H:i:s', strtotime($CreationDate));
        $out['Kategori'] = $Kategori;
        $out['Titel'] = $Title;
        $out['Body'] = $Body;
        $out['Status'] = $Status;
        $out['Fornavn'] = $Fornavn;
        $out['Efternavn'] = $Efternavn;
        $out['Rolle'] = $Rolle;
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