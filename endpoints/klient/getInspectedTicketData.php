<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: GET");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");


if(!isset($_GET['token']) || !isset($_GET['ticketID'])){
    $out['Result'] = 0;
    $out['Message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_GET['token'];
$ticketID = $_GET['ticketID'];

$supporterToken = CheckSupporterToken($givenToken);
if($supporterToken['Result'] != 1){
    $out['Result'] = 0;
    $out['Message'] = "Kun for personale";
    $json = json_encode($out);
    print_r($json);
    return;
}

$database = new Database();
$db = $database->getConnection();

$query = "SELECT u.Fornavn, u.Efternavn, tickets.Title, tickets.Body, tickets.CreationDate, ts.Navn AS Status,";
$query .= " tickets.`Status` AS StatusID, tc.Navn AS Kategori, tickets.Kategori AS KategoriID, ur.Navn AS RolleNavn FROM tickets";
$query .= " INNER JOIN users AS u ON u.ID = tickets.UserID";
$query .= " INNER JOIN ticket_status AS ts ON ts.ID = tickets.`Status`";
$query .= " INNER JOIN ticket_categories AS tc ON tc.ID = tickets.Kategori";
$query .= " INNER JOIN user_roles AS ur ON ur.ID = u.Rolle";
$query .= " WHERE tickets.ID = $ticketID";


$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $num = $stmt->rowCount();
    $out = array();
    if($num > 0){
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        extract($row);
        $out['Fornavn'] = $Fornavn;
        $out['Efternavn'] = $Efternavn;
        $out['Titel'] = $Title;
        $out['Body'] = $Body;
        $out['CreationDate'] = date('d/m/Y', strtotime($CreationDate));
        $out['Klok'] = date('H:i:s', strtotime($CreationDate));
        $out['Status'] = $Status;
        $out['StatusID'] = (int)$StatusID;
        $out['Kategori'] = $Kategori;
        $out['KategoriID'] = (int)$KategoriID;
        $out['RolleNavn'] = $RolleNavn;
        $out['Result'] = 1;
    }else{
        $out['Result'] = 0;
        $out['Message'] = "Ingen resultater fundet";
    }
} catch(PDOException $e){
    $out['Result'] = 0;
    $out['Message'] = $e->getMessage();
}
$json = json_encode($out);
print_r($json);
return;
?>