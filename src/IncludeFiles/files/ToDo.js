
function ChangeState(assignableID, stateID)
{
    var row = this.document.getElementById("row" + assignableID);
    row.style.display = "none";
    window.external.ChangeState(assignableID, stateID);
}