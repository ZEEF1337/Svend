<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: GET");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");


if(!isset($_GET['token'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_GET['token'];

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

$UserID = getUserIDFromToken($givenToken);


$query = "SELECT t.ID, t.CreationDate, tc.Navn AS Kategori, t.Title, ts.Navn AS Status FROM ticket_assigned";
$query .= " INNER JOIN tickets AS t ON t.ID = ticket_assigned.TicketID";
$query .= " INNER JOIN ticket_categories AS tc ON tc.ID = t.Kategori";
$query .= " INNER JOIN ticket_status AS ts ON ts.ID = t.`Status`";
$query .= " WHERE ticket_assigned.UserID = $UserID ORDER BY t.ID DESC;";

if($supporterToken['Rolle'] == 1){
    $query = "SELECT tickets.ID, tickets.CreationDate, tc.Navn AS Kategori, tickets.Title, ts.Navn AS Status FROM tickets";
    $query .= " INNER JOIN ticket_categories AS tc ON tc.ID = tickets.Kategori";
    $query .= " INNER JOIN ticket_status AS ts ON ts.ID = tickets.`Status` ORDER BY tickets.ID DESC;";
}



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
                "ID" => (int)$ID,
                "CreationDate" => date('d/m/Y', strtotime($CreationDate)),
                "Kategori" => $Kategori,
                "Titel" => $Title,
                "Status" => $Status,
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