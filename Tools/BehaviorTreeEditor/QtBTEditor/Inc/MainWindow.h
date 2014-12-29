#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QApplication>
#include <QAction>
#include <QMenu>
#include <QFileDialog>
#include <QFile>
#include <QTextStream>
#include <QToolBar>
#include <QTextEdit>
#include <QMainWindow>
#include <QString>
#include <QMenuBar>

#include "BTEditor.h"

class LogWidget;
class ProjectWidget;
class CenterTabWidget;
class DragWidget;

namespace Ui
{
	class MainWindow;
}

class MainWindow : public QMainWindow
{
	Q_OBJECT

public:
	explicit MainWindow(QWidget *parent = 0);
	~MainWindow();

	void createMenus();
	void createActions();
	void createToolBars();
	void loadFile(QString fileName);

public slots:
	void slotNewFile();
	void slotOpenFile();
	//void slotSaveFile();

private:
	QMenu *fileMenu;
	QMenu *editMenu;
	QMenu *aboutMenu;
	QString fileName;

	QToolBar *fileTool;
	QToolBar *editTool;

	QAction *fileOpenAction;
	QAction *fileNewAction;
	QAction *fileSaveAction;
	QAction *exitAction;
	QAction *copyAction;
	QAction *cutAction;
	QAction *pasteAction;
	QAction *aboutAction;

	QTextEdit *text;

private:
	Aaa m_aaa;
	Ui::MainWindow *m_ui;

	ProjectWidget* m_projectWidget;
	CenterTabWidget* m_centerTabWidget;
	LogWidget* m_logWidget;
	DragWidget* m_dragWidget;

private:
	void connectAction();
	void createDockWidget();
};

#endif // MAINWINDOW_H