<?php
include_once ($_SERVER['DOCUMENT_ROOT']."database.inc");

function generateRandomSalt($len) {
    $database = new Database();
    $db = $database->getConnection();
    $char = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@$#%&/';
    $charLen = strlen($char);
    $saltInUse = 1;
    try{
        while($saltInUse){
            $randomSalt = '';
            for ($i = 0; $i < $len; $i++) {
                $randomSalt .= $char[rand(0, $charLen - 1)];
            }
            $query = "SELECT Salt FROM users WHERE Salt = '$randomSalt'";
            $stmt = $db->prepare($query);
            $stmt->execute();
            $num = $stmt->rowCount();
            if($num>0){
                $saltInUse = 1;
            }else{
                $saltInUse = 0;
            }
        }
        return $randomSalt;
    } catch(PDOException $e){
        return($e);
    }
}

function emailExists($mail){
    $database = new Database();
    $db = $database->getConnection();
    $query = "SELECT Brugernavn FROM users WHERE Email = '$mail'";
    $stmt = $db->prepare($query);
    try{
        $stmt->execute();
        $num = $stmt->rowCount();
        if($num>0){
            return true;
        }else{
            return false;
        }
    } catch(PDOException $e){
        return($e);
    }
}

function userExists($Username){
    $database = new Database();
    $db = $database->getConnection();
    $query = "SELECT Brugernavn FROM users WHERE Brugernavn = '$Username'";
    $stmt = $db->prepare($query);
    try{
        $stmt->execute();
        $num = $stmt->rowCount();
        if($num>0){
            return 1;
        }else{
            return 0;
        }
    } catch(PDOException $e){
        return($e);
    }
}

function verifyPass($username, $pass){
    $salt = getSaltFromDB($username);
    $dbPass = getPasswordFromDB($username);
    $testPass = hash('sha512', hash('sha512', $salt).$pass);
    if(strcmp($dbPass, $testPass) == 0){
        return 1;
    }else{
        return 0;
    }
}

function getSaltFromDB($username){
    $database = new Database();
    $db = $database->getConnection();
    $query = "SELECT Salt FROM users WHERE Brugernavn = '$username'";
    $stmt = $db->prepare($query);
    try{
        $stmt->execute();
        $num = $stmt->rowCount();
        if($num>0){
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            extract($row);
            return $Salt;
        }
    } catch(PDOException $e){
        return($e);
    }
}

function getPasswordFromDB($username){
    $database = new Database();
    $db = $database->getConnection();
    $query = "SELECT Password FROM users WHERE Brugernavn = '$username'";
    $stmt = $db->prepare($query);
    try{
        $stmt->execute();
        $num = $stmt->rowCount();
        if($num>0){
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            extract($row);
            return $Password;
        }
    } catch(PDOException $e){
        return($e);
    }
}

function getFirstLastNameFromUsername($username){
    $database = new Database();
    $db = $database->getConnection();
    $query = "SELECT Fornavn, Efternavn FROM users WHERE Brugernavn = '$username'";
    $stmt = $db->prepare($query);
    try{
        $stmt->execute();
        $num = $stmt->rowCount();
        if($num>0){
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            extract($row);
            $out['Firstname'] = $Fornavn;
            $out['Lastname'] = $Efternavn;
            return $out;
        }
    } catch(PDOException $e){
        return($e);
    }
}
?>