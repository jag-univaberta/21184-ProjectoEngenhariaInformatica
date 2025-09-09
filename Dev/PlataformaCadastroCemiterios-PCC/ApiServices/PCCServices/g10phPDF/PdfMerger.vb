Imports System
Imports System.Collections.Generic
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class PdfMerger
    Private Shared ReadOnly _locker As New Object()

    Public Shared Function MergeFiles(sourceFiles As List(Of Byte())) As Byte()
        SyncLock _locker


            Dim document As New Document()
            Dim output As New MemoryStream()

            Try
                ' Initialize pdf writer
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
                writer.PageEvent = New PdfPageEvents()

                ' Open document to write
                document.Open()
                Dim content As PdfContentByte = writer.DirectContent

                ' Iterate through all pdf documents
                For fileCounter As Integer = 0 To sourceFiles.Count - 1
                    ' Create pdf reader
                    Dim reader As New PdfReader(sourceFiles(fileCounter))
                    Dim numberOfPages As Integer = reader.NumberOfPages

                    ' Iterate through all pages
                    For currentPageIndex As Integer = 1 To numberOfPages
                        ' Determine page size for the current page
                        document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex))

                        ' Create page
                        document.NewPage()
                        Dim importedPage As PdfImportedPage = writer.GetImportedPage(reader, currentPageIndex)


                        ' Determine page orientation
                        Dim pageOrientation As Integer = reader.GetPageRotation(currentPageIndex)
                        If (pageOrientation = 90) OrElse (pageOrientation = 270) Then
                            content.AddTemplate(importedPage, 0, -1.0F, 1.0F, 0, 0, _
                             reader.GetPageSizeWithRotation(currentPageIndex).Height)
                        Else
                            content.AddTemplate(importedPage, 1.0F, 0, 0, 1.0F, 0, _
                             0)
                        End If
                    Next
                Next
            Catch exception As Exception
                Throw New Exception("There has an unexpected exception" & " occured during the pdf merging process.", exception)
            Finally
                document.Close()
            End Try
            Return output.GetBuffer()
        End SyncLock
    End Function

    Public Shared Function MergeFiles(sourceFiles As List(Of String)) As Byte()
        SyncLock _locker
            Dim document As New Document()
            Dim output As New MemoryStream()

            Try

                Dim filesByte As New List(Of Byte())()
                For Each file1 As String In sourceFiles
                    filesByte.Add(System.IO.File.ReadAllBytes(file1))
                Next

                ' Initialize pdf writer
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, output)
                writer.PageEvent = New PdfPageEvents()

                ' Open document to write
                document.Open()
                Dim content As PdfContentByte = writer.DirectContent

                ' Iterate through all pdf documents
                For fileCounter As Integer = 0 To sourceFiles.Count - 1
                    ' Create pdf reader
                    Dim reader As New PdfReader(sourceFiles(fileCounter))
                    Dim numberOfPages As Integer = reader.NumberOfPages

                    ' Iterate through all pages
                    For currentPageIndex As Integer = 1 To numberOfPages
                        ' Determine page size for the current page
                        document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex))

                        ' Create page
                        document.NewPage()
                        Dim importedPage As PdfImportedPage = writer.GetImportedPage(reader, currentPageIndex)


                        ' Determine page orientation
                        Dim pageOrientation As Integer = reader.GetPageRotation(currentPageIndex)
                        If (pageOrientation = 90) OrElse (pageOrientation = 270) Then
                            content.AddTemplate(importedPage, 0, -1.0F, 1.0F, 0, 0, _
                             reader.GetPageSizeWithRotation(currentPageIndex).Height)
                        Else
                            content.AddTemplate(importedPage, 1.0F, 0, 0, 1.0F, 0, _
                             0)
                        End If
                    Next
                Next
            Catch exception As Exception
                Throw New Exception("There has an unexpected exception" & " occured during the pdf merging process.", exception)
            Finally
                document.Close()
            End Try
            Return output.GetBuffer()
        End SyncLock
    End Function
End Class
Friend Class PdfPageEvents
    Implements IPdfPageEvent
#Region "members"
    Private _baseFont As BaseFont = Nothing
    Private _content As PdfContentByte
#End Region
    Private Shared ReadOnly _locker As New Object()


#Region "IPdfPageEvent Members"
    Public Sub OnOpenDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnOpenDocument
        SyncLock _locker
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
            _content = writer.DirectContent
        End SyncLock
    End Sub

    Public Sub OnStartPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnStartPage
    End Sub

    Public Sub OnEndPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnEndPage
        SyncLock _locker
            ' Write header text
            Dim headerText As String = "" '"PDF Merger"
            _content.BeginText()
            _content.SetFontAndSize(_baseFont, 8)
            _content.SetTextMatrix(GetCenterTextPosition(headerText, writer), writer.PageSize.Height - 10)
            _content.ShowText(headerText)
            _content.EndText()

            ' Write footer text (page numbers)
            Dim text As String = "" ' "Página " & Convert.ToString(writer.PageNumber)
            _content.BeginText()
            _content.SetFontAndSize(_baseFont, 8)
            _content.SetTextMatrix(GetCenterTextPosition(text, writer), 10)
            _content.ShowText(text)
            _content.EndText()
        End SyncLock
    End Sub

    Public Sub OnCloseDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnCloseDocument
    End Sub

    Public Sub OnParagraph(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnParagraph
    End Sub

    Public Sub OnParagraphEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnParagraphEnd
    End Sub

    Public Sub OnChapter(writer As PdfWriter, document As Document, paragraphPosition As Single, title As Paragraph) Implements IPdfPageEvent.OnChapter
    End Sub

    Public Sub OnChapterEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnChapterEnd
    End Sub

    Public Sub OnSection(writer As PdfWriter, document As Document, paragraphPosition As Single, depth As Integer, title As Paragraph) Implements IPdfPageEvent.OnSection
    End Sub

    Public Sub OnSectionEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnSectionEnd
    End Sub

    Public Sub OnGenericTag(writer As PdfWriter, document As Document, rect As Rectangle, text As String) Implements IPdfPageEvent.OnGenericTag
    End Sub
#End Region

    Private Function GetCenterTextPosition(text As String, writer As PdfWriter) As Single
        SyncLock _locker
            Return writer.PageSize.Width / 2 - _baseFont.GetWidthPoint(text, 8) / 2
        End SyncLock
    End Function
End Class
