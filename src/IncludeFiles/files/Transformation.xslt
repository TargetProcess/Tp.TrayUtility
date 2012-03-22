
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.targetprocees.com/" exclude-result-prefixes="msxsl user">
	<xsl:template match="/MyAssignments">
		<html>
			<head>
				<link href="Main.css" type="text/css" rel="stylesheet"/>
		<script src="ToDo.js" type="text/javascript"></script>
	  </head>
			<body>
				<div class="main">
					<div class="table">
						<table class="generalTable" id="assignables" style="border-collapse: collapse" cellspacing="0" cellpadding="4" border="0" width="100%">
							<tbody>

								<xsl:apply-templates select="Projects/ProjectSimpleDTO"/>

								<xsl:if test="count(Assignables/AssignableSimpleDTO) = 0">
									<tr>
										<td colspan="4">You don't have any assignments for now</td>
									</tr>
								</xsl:if>
							</tbody>
						</table>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="ProjectSimpleDTO">
		<xsl:variable name="projectid" select="ID"/>
		<xsl:if test="count(../../Assignables[AssignableSimpleDTO/ProjectID=$projectid]) &gt; 0">
			<tr>
				<td colspan="4">
					<h2>
						<xsl:value-of select="Name"></xsl:value-of>
					</h2>
				</td>
			</tr>
			<xsl:apply-templates select="../../Assignables/AssignableSimpleDTO[ProjectID=$projectid]">
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>

	<xsl:template match="AssignableSimpleDTO">
		<xsl:variable name="entitystateid" select="EntityStateID"/>
		<tr>
			<xsl:attribute name="id">row<xsl:value-of select="AssignableID"/></xsl:attribute>
			<td width="1%">
		<img style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;border-right-width: 0px">
		  <xsl:attribute name="src">
			<xsl:value-of select="IconPath"/>
		  </xsl:attribute>
			<!--user:GetIcon();
			 <xsl:value-of select="EntityTypeID"/>.gif</xsl:attribute>-->
				</img>
			</td>
			<td class="big">
				<xsl:value-of select="Name"/>
			</td>
			<td class="small" style="white-space: nowrap" align="right">
				<select class="input">
					<xsl:attribute name="onchange">ChangeState(<xsl:value-of select="AssignableID"/>, this.options[this.selectedIndex].value);</xsl:attribute>
					<option selected="selected">
						<xsl:attribute name="value">
							<xsl:value-of select="EntityStateID"/>
						</xsl:attribute>
						<xsl:value-of select="../../States/EntityStateDTO/Name[../ID=$entitystateid]"/>
					</option>
					<xsl:variable name="nextStates" select="../../States/EntityStateDTO/NextStates[../ID=$entitystateid]"></xsl:variable>
					<xsl:for-each select="../../States/EntityStateDTO">
						<xsl:if test="user:IsNextState($nextStates, EntityStateID)">
							<option>
								<xsl:attribute name="value">
									<xsl:value-of select="EntityStateID"/>
								</xsl:attribute>
								<xsl:value-of select="Name"/>
							</option>
						</xsl:if>
					</xsl:for-each>
				</select>
			</td>
		</tr>
	</xsl:template>

	<msxsl:script language="C#" implements-prefix="user">
	<![CDATA[
  public bool IsNextState(string nextStates, string stateId)
  {
	 string[] states = nextStates.Split(',');
	 foreach(string state in states)
	 {
		if (state == stateId)
			return true;
	 }

	 return false;
  }
  
  ]]>
	</msxsl:script>
</xsl:stylesheet>