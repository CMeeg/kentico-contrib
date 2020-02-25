CREATE OR ALTER PROCEDURE Proc_Meeg_Configuration_AllConfigCmsSettings
	@KeyNamePrefix nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		KeyName,
		KeyValue,
		CategoryName,
		SiteName
	FROM
		CMS_SettingsKey sk
	LEFT JOIN
		CMS_SettingsCategory sc ON sk.KeyCategoryID = sc.CategoryID
	LEFT JOIN
		CMS_Site s ON sk.SiteID = s.SiteID
	WHERE
		KeyName LIKE @KeyNamePrefix
	ORDER BY
		ISNULL(sk.SiteID, 0) DESC,
		KeyName ASC
END
GO
