Use EviweKhohlombe
Go
Create Procedure AspNetUser
@Email varchar(100)
As
Begin
Select * From AspNetUsers
Where UserName = @Email;
End

