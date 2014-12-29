#include "MainWindow.h"
#include "ui_MainWindow.h"

#include "LogWidget.h"
#include "ProjectWidget.h"
#include "CenterTabWidget.h"

#include <QtCore/qnamespace.h>

MainWindow::MainWindow(QWidget *parent)
	: QMainWindow(parent, 0), m_ui(new Ui::MainWindow)
{
	m_ui->setupUi(this);

	//设置主窗口标题
	setWindowTitle(QStringLiteral("行为树编辑器"));
	//text = new QTextEdit(this);
	//setCentralWidget(text);

	//createActions();
	//createMenus();
	//createToolBars();

	connectAction();
	createDockWidget();

	m_aaa.sayHello();
}

MainWindow::~MainWindow()
{

}

void MainWindow::createDockWidget()
{
	m_projectWidget = new ProjectWidget(this);
	this->addDockWidget(Qt::LeftDockWidgetArea, m_projectWidget);

	//m_centerTabWidget = new CenterTabWidget(this);
	//this->addDockWidget(Qt::RightDockWidgetArea, m_centerTabWidget);

	m_logWidget = new LogWidget(this);
	this->addDockWidget(Qt::BottomDockWidgetArea, m_logWidget);
}

void MainWindow::createActions()
{
	//通过图片路径来实现添加Qt资源文件目的
	fileOpenAction = new QAction(QIcon("C:/qt/Mwindow/open.png"),tr("Open"),this);
	fileOpenAction->setShortcut(tr("Ctrl+O"));
	fileOpenAction->setStatusTip(tr("Open a file"));
	connect(fileOpenAction,SIGNAL(triggered()),this,SLOT(slotOpenFile()));

	fileNewAction = new QAction(QIcon("C:/qt/Mwindow/new.png"),tr("New"),this);
	fileNewAction->setShortcut(tr("Ctrl+N"));
	fileNewAction->setStatusTip(tr("new file"));
	connect(fileNewAction,SIGNAL(triggered()),this,SLOT(slotNewFile()));
  
	fileSaveAction = new QAction(QIcon("C:/qt/Mwindow/save.png"),tr("Save"),this);
	fileSaveAction->setShortcut(tr("Ctrl+S"));
	fileNewAction->setStatusTip(tr("save file"));
	connect(fileNewAction,SIGNAL(triggered()),this,SLOT(slotSaveFile()));

	exitAction = new QAction(tr("Exit"),this);
	exitAction->setShortcut(tr("Ctrl+Q"));
	exitAction->setStatusTip(tr("exit"));
	connect(exitAction,SIGNAL(triggered()),this,SLOT(close()));

	cutAction = new QAction(QIcon("C:/qt/Mwindow/cut.png"),tr("Cut"),this);
	cutAction->setShortcut(tr("Ctrl+X"));
	cutAction->setStatusTip(tr("cut to clipboard"));
	connect(cutAction,SIGNAL(triggered()),text,SLOT(cut()));

	copyAction = new QAction(QIcon("C:/qt/Mwindow/copy.png"),tr("Copy"),this);
	copyAction->setShortcut(tr("Ctrl+C"));
	copyAction->setStatusTip(tr("copy to clipboard"));
	connect(copyAction,SIGNAL(triggered()),this,SLOT(copy()));

	pasteAction = new QAction(QIcon("C:/qt/Mwindow/paste.png"),tr("paste"),this);
	pasteAction->setShortcut(tr("Ctrl+V"));
	pasteAction->setStatusTip(tr("paste clipboard to selection"));
	connect(pasteAction,SIGNAL(triggered()),this,SLOT(paste()));

	aboutAction = new QAction(tr("About"),this);
	connect(aboutAction,SIGNAL(triggered()),this,SLOT(slotAbout()));
}

void MainWindow::createMenus()
{
	fileMenu = menuBar()->addMenu(tr("File"));
	fileMenu->addAction(fileNewAction);
	fileMenu->addAction(fileOpenAction);
	fileMenu->addAction(fileSaveAction);
	fileMenu->addAction(exitAction);

	editMenu = menuBar()->addMenu(tr("Edit"));
	editMenu->addAction(copyAction);
	editMenu->addAction(cutAction);
	editMenu->addAction(pasteAction);

	aboutMenu = menuBar()->addMenu(tr("Help"));
	aboutMenu->addAction(aboutAction);
}

void MainWindow::createToolBars()
{
	fileTool = addToolBar("File");
	fileTool->addAction(fileNewAction);
	fileTool->addAction(fileOpenAction);
	fileTool->addAction(fileSaveAction);

	editTool = addToolBar("Edit");
	editTool->addAction(copyAction);
	editTool->addAction(cutAction);
	editTool->addAction(pasteAction);
}

void MainWindow::slotNewFile()
{
	MainWindow *newWin = new MainWindow;
	newWin->show();
}

void MainWindow::slotOpenFile()
{
	fileName = QFileDialog::getOpenFileName(this);
	if(!fileName.isEmpty())
	{
		if(text->document()->isEmpty())
		{
			loadFile(fileName);
		}
		else
		{
			MainWindow *newWin = new MainWindow;
			newWin->show();
			newWin->loadFile(fileName);
		}
	}
}

void MainWindow::loadFile(QString fileName)
{
	printf("file name:%s\n",fileName.data());
	QFile file(fileName);
	if(file.open(QIODevice::ReadOnly|QIODevice::Text))
	{
		QTextStream textStream(&file);
		while(!textStream.atEnd())
		{
			text->append(textStream.readLine());
			printf("read line.\n");
		}
		printf("end\n");
	}
}

void MainWindow::connectAction()
{
	QObject::connect(m_ui->actionOpen, SIGNAL(triggered()), this, SLOT(slotOpenFile()));
}